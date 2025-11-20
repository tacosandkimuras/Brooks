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
using NinjaTrader.NinjaScript.Indicators;
#endregion

/// <summary>
/// Al Brooks H2 and L2 Pullback Strategy - Automated Trading
/// Automatically enters positions on H2 (High 2) bull flag and L2 (Low 2) bear flag patterns
/// Entry: 1 tick above H2 signal bar high (long) or 1 tick below L2 signal bar low (short)
/// Stop: 1 tick below signal bar low (long) or 1 tick above signal bar high (short)
/// </summary>
namespace NinjaTrader.NinjaScript.Strategies
{
    public class BrooksH2L2Strategy : Strategy
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
        private bool inBullTrend = false;
        private bool inBearTrend = false;
        private int lastSwingHighBar = -1;
        private int lastSwingLowBar = -1;
        
        // Order tracking
        private Order entryOrder = null;
        private Order stopOrder = null;
        #endregion

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"Al Brooks H2 and L2 Pullback Strategy - Automatically enters H2 long and L2 short trades with stop loss orders";
                Name = "BrooksH2L2Strategy";
                Calculate = Calculate.OnBarClose;
                EntriesPerDirection = 1;
                EntryHandling = EntryHandling.AllEntries;
                IsExitOnSessionCloseStrategy = true;
                ExitOnSessionCloseSeconds = 30;
                IsFillLimitOnTouch = false;
                MaximumBarsLookBack = MaximumBarsLookBack.TwoHundredFiftySix;
                OrderFillResolution = OrderFillResolution.Standard;
                Slippage = 0;
                StartBehavior = StartBehavior.WaitUntilFlat;
                TimeInForce = TimeInForce.Gtc;
                TraceOrders = false;
                RealtimeErrorHandling = RealtimeErrorHandling.StopCancelClose;
                StopTargetHandling = StopTargetHandling.PerEntryExecution;
                BarsRequiredToTrade = 20;
                IsInstantiatedOnEachOptimizationIteration = true;
                
                // Parameters
                EmaPeriod = 20;
                SwingStrength = 5;
                Quantity = 1;
                EnableAlerts = true;
                
                // Visual settings
                H2SignalColor = Brushes.Lime;
                L2SignalColor = Brushes.Red;
            }
            else if (State == State.Configure)
            {
            }
            else if (State == State.DataLoaded)
            {
                // Initialize EMA
                ema = EMA(EmaPeriod);
                
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
            
            // Detect H2/L2 patterns and place orders
            DetectPatternsAndTrade();
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
            }
        }
        
        /// <summary>
        /// Detect H2 and L2 pullback patterns and place trades
        /// </summary>
        private void DetectPatternsAndTrade()
        {
            // H2 Pattern: Two-legged pullback in bull trend
            if (inBullTrend && swingLows.Count >= 2 && Position.MarketPosition == MarketPosition.Flat)
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
                        // H2 Signal - Place Long Entry Order
                        double entryPrice = High[0] + TickSize;  // 1 tick above signal bar high
                        double stopPrice = Low[0] - TickSize;     // 1 tick below signal bar low
                        
                        // Enter long with stop loss
                        EnterLongStopMarket(0, true, Quantity, entryPrice, "H2Long");
                        SetStopLoss("H2Long", CalculationMode.Price, stopPrice, false);
                        
                        // Visual marker
                        Draw.ArrowUp(this, "H2_" + CurrentBar, true, 0, Low[0] - TickSize * 2, H2SignalColor);
                        Draw.Text(this, "H2Text_" + CurrentBar, "H2", 0, Low[0] - TickSize * 4, H2SignalColor);
                        
                        if (EnableAlerts)
                        {
                            Alert("H2Alert_" + CurrentBar, Priority.High, 
                                "H2 Long Entry: " + entryPrice.ToString("F2") + " Stop: " + stopPrice.ToString("F2"), 
                                NinjaTrader.Core.Globals.InstallDir + @"\sounds\Alert1.wav", 10, 
                                Brushes.Lime, Brushes.Black);
                        }
                    }
                }
            }
            
            // L2 Pattern: Two-legged pullback in bear trend
            if (inBearTrend && swingHighs.Count >= 2 && Position.MarketPosition == MarketPosition.Flat)
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
                        // L2 Signal - Place Short Entry Order
                        double entryPrice = Low[0] - TickSize;   // 1 tick below signal bar low
                        double stopPrice = High[0] + TickSize;   // 1 tick above signal bar high
                        
                        // Enter short with stop loss
                        EnterShortStopMarket(0, true, Quantity, entryPrice, "L2Short");
                        SetStopLoss("L2Short", CalculationMode.Price, stopPrice, false);
                        
                        // Visual marker
                        Draw.ArrowDown(this, "L2_" + CurrentBar, true, 0, High[0] + TickSize * 2, L2SignalColor);
                        Draw.Text(this, "L2Text_" + CurrentBar, "L2", 0, High[0] + TickSize * 4, L2SignalColor);
                        
                        if (EnableAlerts)
                        {
                            Alert("L2Alert_" + CurrentBar, Priority.High, 
                                "L2 Short Entry: " + entryPrice.ToString("F2") + " Stop: " + stopPrice.ToString("F2"), 
                                NinjaTrader.Core.Globals.InstallDir + @"\sounds\Alert2.wav", 10, 
                                Brushes.Red, Brushes.Black);
                        }
                    }
                }
            }
        }
        
        #endregion
        
        #region Order Events
        
        protected override void OnOrderUpdate(Order order, double limitPrice, double stopPrice, int quantity, int filled, double averageFillPrice, OrderState orderState, DateTime time, ErrorCode error, string nativeError)
        {
            // Track orders for debugging
            if (order.Name == "H2Long" || order.Name == "L2Short")
            {
                entryOrder = order;
                
                // Print order status for debugging
                if (orderState == OrderState.Filled)
                {
                    Print(string.Format("{0} {1} filled at {2}", Time[0], order.Name, averageFillPrice));
                }
            }
        }
        
        protected override void OnExecutionUpdate(Execution execution, string executionId, double price, int quantity, MarketPosition marketPosition, string orderId, DateTime time)
        {
            // Handle execution updates
            if (execution.Order.Name == "H2Long" || execution.Order.Name == "L2Short")
            {
                Print(string.Format("{0} {1} executed: {2} @ {3}", Time[0], execution.Order.Name, quantity, price));
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
        [Range(1, int.MaxValue)]
        [Display(Name = "Quantity", Description = "Number of contracts to trade per signal", Order = 3, GroupName = "Parameters")]
        public int Quantity
        { get; set; }

        [NinjaScriptProperty]
        [Display(Name = "Enable Alerts", Description = "Enable audio alerts for H2/L2 signals", Order = 4, GroupName = "Alerts")]
        public bool EnableAlerts
        { get; set; }

        [XmlIgnore]
        [Display(Name = "H2 Signal Color", Description = "Color for H2 bull flag signals", Order = 1, GroupName = "Visual Colors")]
        public Brush H2SignalColor
        { get; set; }

        [Browsable(false)]
        public string H2SignalColorSerializable
        {
            get { return Serialize.BrushToString(H2SignalColor); }
            set { H2SignalColor = Serialize.StringToBrush(value); }
        }

        [XmlIgnore]
        [Display(Name = "L2 Signal Color", Description = "Color for L2 bear flag signals", Order = 2, GroupName = "Visual Colors")]
        public Brush L2SignalColor
        { get; set; }

        [Browsable(false)]
        public string L2SignalColorSerializable
        {
            get { return Serialize.BrushToString(L2SignalColor); }
            set { L2SignalColor = Serialize.StringToBrush(value); }
        }

        #endregion
    }
} 

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		private BrooksH2L2Strategy[] cacheBrooksH2L2Strategy;
		public BrooksH2L2Strategy BrooksH2L2Strategy(int emaPeriod, int swingStrength, int quantity, bool enableAlerts)
		{
			return BrooksH2L2Strategy(Input, emaPeriod, swingStrength, quantity, enableAlerts);
		}

		public BrooksH2L2Strategy BrooksH2L2Strategy(ISeries<double> input, int emaPeriod, int swingStrength, int quantity, bool enableAlerts)
		{
			if (cacheBrooksH2L2Strategy != null)
				for (int idx = 0; idx < cacheBrooksH2L2Strategy.Length; idx++)
					if (cacheBrooksH2L2Strategy[idx] != null && cacheBrooksH2L2Strategy[idx].EmaPeriod == emaPeriod && cacheBrooksH2L2Strategy[idx].SwingStrength == swingStrength && cacheBrooksH2L2Strategy[idx].Quantity == quantity && cacheBrooksH2L2Strategy[idx].EnableAlerts == enableAlerts)
						return cacheBrooksH2L2Strategy[idx];
			return CacheIndicator<BrooksH2L2Strategy>(new BrooksH2L2Strategy(){ EmaPeriod = emaPeriod, SwingStrength = swingStrength, Quantity = quantity, EnableAlerts = enableAlerts }, input, ref cacheBrooksH2L2Strategy);
		}
	}
}

#endregion