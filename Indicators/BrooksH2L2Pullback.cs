#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

/// <summary>
/// Al Brooks H2 and L2 Pullback Indicator
/// Identifies High 2 (H2) bull flag and Low 2 (L2) bear flag patterns based on Al Brooks' price action methodology
/// H2: Second entry long in bull trend after two-legged pullback
/// L2: Second entry short in bear trend after two-legged pullback
/// </summary>
namespace NinjaTrader.NinjaScript.Indicators
{
    public class BrooksH2L2Pullback : Indicator
    {
        #region Variables
        private EMA ema;
        private Swing swing;
        
        // Tracking swing points
        private List<double> swingHighs = new List<double>();
        private List<int> swingHighBars = new List<int>();
        private List<double> swingLows = new List<double>();
        private List<int> swingLowBars = new List<int>();
        
        // Pattern tracking
        private int pullbackLegsDown = 0;
        private int pullbackLegsUp = 0;
        private bool inBullTrend = false;
        private bool inBearTrend = false;
        private int lastSwingHighBar = -1;
        private int lastSwingLowBar = -1;
        #endregion

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"Al Brooks H2 and L2 Pullback Indicator - Identifies High 2 bull flags and Low 2 bear flags for trend continuation entries";
                Name = "BrooksH2L2Pullback";
                Calculate = Calculate.OnBarClose;
                IsOverlay = true;
                DisplayInDataBox = true;
                DrawOnPricePanel = true;
                DrawHorizontalGridLines = true;
                DrawVerticalGridLines = true;
                PaintPriceMarkers = true;
                ScaleJustification = NinjaTrader.Gui.Chart.ScaleJustification.Right;
                IsSuspendedWhileInactive = true;
                
                // Parameters
                EmaPeriod = 20;
                SwingStrength = 5;
                ShowEMA = true;
                ShowSwings = true;
                EnableAlerts = true;
                
                // Visual settings
                EmaColor = Brushes.Yellow;
                H2SignalColor = Brushes.Lime;
                L2SignalColor = Brushes.Red;
                SwingHighColor = Brushes.Cyan;
                SwingLowColor = Brushes.Magenta;
            }
            else if (State == State.Configure)
            {
            }
            else if (State == State.DataLoaded)
            {
                // Initialize EMA
                ema = EMA(EmaPeriod);
                AddChartIndicator(ema);
                
                // Initialize Swing indicator
                swing = Swing(SwingStrength);
                
                // Clear lists
                swingHighs.Clear();
                swingHighBars.Clear();
                swingLows.Clear();
                swingLowBars.Clear();
            }
        }

        protected override void OnBarUpdate()
        {
            if (CurrentBar < EmaPeriod + SwingStrength * 2)
                return;
            
            // Determine trend direction based on price relative to EMA
            DetermineTrend();
            
            // Detect swing highs and lows
            DetectSwings();
            
            // Count pullback legs and detect H2/L2 patterns
            DetectPullbackPatterns();
        }

        #region Helper Methods
        
        /// <summary>
        /// Determine if we are in a bull or bear trend based on EMA
        /// </summary>
        private void DetermineTrend()
        {
            // Bull trend: price is above EMA and EMA is rising
            if (Close[0] > ema[0] && ema[0] > ema[1])
            {
                inBullTrend = true;
                inBearTrend = false;
            }
            // Bear trend: price is below EMA and EMA is falling
            else if (Close[0] < ema[0] && ema[0] < ema[1])
            {
                inBullTrend = false;
                inBearTrend = true;
            }
            // Otherwise maintain previous trend state but with less confidence
        }
        
        /// <summary>
        /// Detect swing highs and lows using the Swing indicator
        /// </summary>
        private void DetectSwings()
        {
            // Check for new swing high
            int barsAgoSwingHigh = swing.SwingHighBar(0, 1, SwingStrength * 2);
            if (barsAgoSwingHigh != -1 && barsAgoSwingHigh != lastSwingHighBar)
            {
                double swingHighPrice = swing.SwingHigh[barsAgoSwingHigh];
                
                swingHighs.Add(swingHighPrice);
                swingHighBars.Add(CurrentBar - barsAgoSwingHigh);
                lastSwingHighBar = barsAgoSwingHigh;
                
                // Keep only recent swings
                if (swingHighs.Count > 10)
                {
                    swingHighs.RemoveAt(0);
                    swingHighBars.RemoveAt(0);
                }
                
                // Visual marker for swing high
                if (ShowSwings)
                {
                    Draw.Dot(this, "SwingHigh_" + CurrentBar, true, barsAgoSwingHigh, 
                        swingHighPrice, SwingHighColor);
                }
            }
            
            // Check for new swing low
            int barsAgoSwingLow = swing.SwingLowBar(0, 1, SwingStrength * 2);
            if (barsAgoSwingLow != -1 && barsAgoSwingLow != lastSwingLowBar)
            {
                double swingLowPrice = swing.SwingLow[barsAgoSwingLow];
                
                swingLows.Add(swingLowPrice);
                swingLowBars.Add(CurrentBar - barsAgoSwingLow);
                lastSwingLowBar = barsAgoSwingLow;
                
                // Keep only recent swings
                if (swingLows.Count > 10)
                {
                    swingLows.RemoveAt(0);
                    swingLowBars.RemoveAt(0);
                }
                
                // Visual marker for swing low
                if (ShowSwings)
                {
                    Draw.Dot(this, "SwingLow_" + CurrentBar, true, barsAgoSwingLow, 
                        swingLowPrice, SwingLowColor);
                }
            }
        }
        
        /// <summary>
        /// Detect H2 and L2 pullback patterns
        /// </summary>
        private void DetectPullbackPatterns()
        {
            // H2 Pattern: Two-legged pullback in bull trend
            if (inBullTrend && swingLows.Count >= 2)
            {
                // Check if we have two consecutive swing lows forming a pullback
                int recentSwingsCount = Math.Min(4, swingLows.Count);
                
                // Look for two distinct down legs in the pullback
                bool hasDownLeg1 = false;
                bool hasDownLeg2 = false;
                
                for (int i = swingLows.Count - 1; i >= Math.Max(0, swingLows.Count - recentSwingsCount); i--)
                {
                    // Check if this swing low is below the EMA (pullback zone)
                    if (swingLows[i] <= ema[CurrentBar - swingLowBars[i] + SwingStrength])
                    {
                        if (!hasDownLeg1)
                            hasDownLeg1 = true;
                        else if (hasDownLeg1 && !hasDownLeg2)
                        {
                            hasDownLeg2 = true;
                            break;
                        }
                    }
                }
                
                // H2 Setup: After second leg down, look for bullish reversal bar
                if (hasDownLeg2)
                {
                    // Check for bullish signal bar (close above open, close near high)
                    bool isBullishBar = Close[0] > Open[0] && 
                                       (Close[0] - Low[0]) > (High[0] - Close[0]) * 0.5;
                    
                    // Price should be moving back toward EMA
                    bool movingTowardEMA = Close[0] > Close[1] && Close[0] > Low[1];
                    
                    if (isBullishBar && movingTowardEMA)
                    {
                        // H2 Signal!
                        Draw.ArrowUp(this, "H2_" + CurrentBar, true, 0, Low[0] - TickSize * 2, H2SignalColor);
                        Draw.Text(this, "H2Text_" + CurrentBar, "H2", 0, Low[0] - TickSize * 4, H2SignalColor);
                        
                        if (EnableAlerts)
                        {
                            Alert("H2Alert_" + CurrentBar, Priority.High, 
                                "H2 Bull Flag Setup", 
                                NinjaTrader.Core.Globals.InstallDir + @"\sounds\Alert1.wav", 10, 
                                Brushes.Lime, Brushes.Black);
                        }
                    }
                }
            }
            
            // L2 Pattern: Two-legged pullback in bear trend
            if (inBearTrend && swingHighs.Count >= 2)
            {
                // Check if we have two consecutive swing highs forming a pullback
                int recentSwingsCount = Math.Min(4, swingHighs.Count);
                
                // Look for two distinct up legs in the pullback
                bool hasUpLeg1 = false;
                bool hasUpLeg2 = false;
                
                for (int i = swingHighs.Count - 1; i >= Math.Max(0, swingHighs.Count - recentSwingsCount); i--)
                {
                    // Check if this swing high is above the EMA (pullback zone)
                    if (swingHighs[i] >= ema[CurrentBar - swingHighBars[i] + SwingStrength])
                    {
                        if (!hasUpLeg1)
                            hasUpLeg1 = true;
                        else if (hasUpLeg1 && !hasUpLeg2)
                        {
                            hasUpLeg2 = true;
                            break;
                        }
                    }
                }
                
                // L2 Setup: After second leg up, look for bearish reversal bar
                if (hasUpLeg2)
                {
                    // Check for bearish signal bar (close below open, close near low)
                    bool isBearishBar = Close[0] < Open[0] && 
                                       (High[0] - Close[0]) > (Close[0] - Low[0]) * 0.5;
                    
                    // Price should be moving back toward EMA
                    bool movingTowardEMA = Close[0] < Close[1] && Close[0] < High[1];
                    
                    if (isBearishBar && movingTowardEMA)
                    {
                        // L2 Signal!
                        Draw.ArrowDown(this, "L2_" + CurrentBar, true, 0, High[0] + TickSize * 2, L2SignalColor);
                        Draw.Text(this, "L2Text_" + CurrentBar, "L2", 0, High[0] + TickSize * 4, L2SignalColor);
                        
                        if (EnableAlerts)
                        {
                            Alert("L2Alert_" + CurrentBar, Priority.High, 
                                "L2 Bear Flag Setup", 
                                NinjaTrader.Core.Globals.InstallDir + @"\sounds\Alert2.wav", 10, 
                                Brushes.Red, Brushes.Black);
                        }
                    }
                }
            }
        }
        
        #endregion

        #region Properties
        
        [NinjaScriptProperty]
        [Range(5, 200)]
        [Display(Name = "EMA Period", Description = "Period for the EMA trend filter (default 20)", Order = 1, GroupName = "Parameters")]
        public int EmaPeriod
        { get; set; }

        [NinjaScriptProperty]
        [Range(1, 10)]
        [Display(Name = "Swing Strength", Description = "Number of bars on each side for swing detection (default 5)", Order = 2, GroupName = "Parameters")]
        public int SwingStrength
        { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Show EMA", Description = "Display the EMA line on chart", Order = 3, GroupName = "Visual")]
        public bool ShowEMA
        { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Show Swings", Description = "Display swing high/low markers", Order = 4, GroupName = "Visual")]
        public bool ShowSwings
        { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Enable Alerts", Description = "Enable audio alerts for H2/L2 signals", Order = 5, GroupName = "Alerts")]
        public bool EnableAlerts
        { get; set; }

        [XmlIgnore]
        [Display(Name = "EMA Color", Description = "Color of the EMA line", Order = 1, GroupName = "Visual Colors")]
        public Brush EmaColor
        { get; set; }

        [Browsable(false)]
        public string EmaColorSerializable
        {
            get { return Serialize.BrushToString(EmaColor); }
            set { EmaColor = Serialize.StringToBrush(value); }
        }

        [XmlIgnore]
        [Display(Name = "H2 Signal Color", Description = "Color for H2 bull flag signals", Order = 2, GroupName = "Visual Colors")]
        public Brush H2SignalColor
        { get; set; }

        [Browsable(false)]
        public string H2SignalColorSerializable
        {
            get { return Serialize.BrushToString(H2SignalColor); }
            set { H2SignalColor = Serialize.StringToBrush(value); }
        }

        [XmlIgnore]
        [Display(Name = "L2 Signal Color", Description = "Color for L2 bear flag signals", Order = 3, GroupName = "Visual Colors")]
        public Brush L2SignalColor
        { get; set; }

        [Browsable(false)]
        public string L2SignalColorSerializable
        {
            get { return Serialize.BrushToString(L2SignalColor); }
            set { L2SignalColor = Serialize.StringToBrush(value); }
        }

        [XmlIgnore]
        [Display(Name = "Swing High Color", Description = "Color for swing high markers", Order = 4, GroupName = "Visual Colors")]
        public Brush SwingHighColor
        { get; set; }

        [Browsable(false)]
        public string SwingHighColorSerializable
        {
            get { return Serialize.BrushToString(SwingHighColor); }
            set { SwingHighColor = Serialize.StringToBrush(value); }
        }

        [XmlIgnore]
        [Display(Name = "Swing Low Color", Description = "Color for swing low markers", Order = 5, GroupName = "Visual Colors")]
        public Brush SwingLowColor
        { get; set; }

        [Browsable(false)]
        public string SwingLowColorSerializable
        {
            get { return Serialize.BrushToString(SwingLowColor); }
            set { SwingLowColor = Serialize.StringToBrush(value); }
        }

        #endregion
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private BrooksH2L2Pullback[] cacheBrooksH2L2Pullback;
		public BrooksH2L2Pullback BrooksH2L2Pullback(int emaPeriod, int swingStrength, bool showEMA, bool showSwings, bool enableAlerts)
		{
			return BrooksH2L2Pullback(Input, emaPeriod, swingStrength, showEMA, showSwings, enableAlerts);
		}

		public BrooksH2L2Pullback BrooksH2L2Pullback(ISeries<double> input, int emaPeriod, int swingStrength, bool showEMA, bool showSwings, bool enableAlerts)
		{
			if (cacheBrooksH2L2Pullback != null)
				for (int idx = 0; idx < cacheBrooksH2L2Pullback.Length; idx++)
					if (cacheBrooksH2L2Pullback[idx] != null && cacheBrooksH2L2Pullback[idx].EmaPeriod == emaPeriod && cacheBrooksH2L2Pullback[idx].SwingStrength == swingStrength && cacheBrooksH2L2Pullback[idx].ShowEMA == showEMA && cacheBrooksH2L2Pullback[idx].ShowSwings == showSwings && cacheBrooksH2L2Pullback[idx].EnableAlerts == enableAlerts && cacheBrooksH2L2Pullback[idx].EqualsInput(input))
						return cacheBrooksH2L2Pullback[idx];
			return CacheIndicator<BrooksH2L2Pullback>(new BrooksH2L2Pullback(){ EmaPeriod = emaPeriod, SwingStrength = swingStrength, ShowEMA = showEMA, ShowSwings = showSwings, EnableAlerts = enableAlerts }, input, ref cacheBrooksH2L2Pullback);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.BrooksH2L2Pullback BrooksH2L2Pullback(int emaPeriod, int swingStrength, bool showEMA, bool showSwings, bool enableAlerts)
		{
			return indicator.BrooksH2L2Pullback(Input, emaPeriod, swingStrength, showEMA, showSwings, enableAlerts);
		}

		public Indicators.BrooksH2L2Pullback BrooksH2L2Pullback(ISeries<double> input , int emaPeriod, int swingStrength, bool showEMA, bool showSwings, bool enableAlerts)
		{
			return indicator.BrooksH2L2Pullback(input, emaPeriod, swingStrength, showEMA, showSwings, enableAlerts);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.BrooksH2L2Pullback BrooksH2L2Pullback(int emaPeriod, int swingStrength, bool showEMA, bool showSwings, bool enableAlerts)
		{
			return indicator.BrooksH2L2Pullback(Input, emaPeriod, swingStrength, showEMA, showSwings, enableAlerts);
		}

		public Indicators.BrooksH2L2Pullback BrooksH2L2Pullback(ISeries<double> input , int emaPeriod, int swingStrength, bool showEMA, bool showSwings, bool enableAlerts)
		{
			return indicator.BrooksH2L2Pullback(input, emaPeriod, swingStrength, showEMA, showSwings, enableAlerts);
		}
	}
}

#endregion
