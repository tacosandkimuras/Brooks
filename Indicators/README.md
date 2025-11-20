# Brooks H2/L2 Pullback Indicator for NinjaTrader 8

## Overview

This NinjaTrader 8 indicator implements Al Brooks' **H2 (High 2) Bull Flag** and **L2 (Low 2) Bear Flag** pullback patterns, which are among his top 10 best price action trading patterns for trend continuation entries.

The indicator automatically identifies:
- **H2 Patterns**: Second entry long setups in bull trends after two-legged pullbacks
- **L2 Patterns**: Second entry short setups in bear trends after two-legged pullbacks

## Al Brooks Price Action Methodology

### H2 Bull Flag Pattern
In a bull trend, after an initial move up:
1. The market pulls back in two distinct legs (swings) down
2. The first attempt to resume the uptrend (H1) often fails
3. The **second attempt (H2)** provides a higher-probability long entry
4. Entry is typically above the high of a strong bullish signal bar after the second pullback leg

### L2 Bear Flag Pattern
In a bear trend, after an initial move down:
1. The market pulls back in two distinct legs (swings) up
2. The first attempt to resume the downtrend (L1) often fails
3. The **second attempt (L2)** provides a higher-probability short entry
4. Entry is typically below the low of a strong bearish signal bar after the second pullback leg

### Why H2/L2 Patterns Work
- **Higher Probability**: Second entry setups have better success rates than first entries
- **Trapped Traders**: First pullback often traps counter-trend traders
- **Trend Confirmation**: Second entry confirms the trend is still in control
- **Good Risk/Reward**: Entry after confirmation with clear stop placement

## Features

### Core Functionality
- **20-bar EMA** for trend identification (configurable)
- **Automatic swing high/low detection** using configurable strength parameter
- **Real-time H2 pattern detection** in bull trends
- **Real-time L2 pattern detection** in bear trends
- **Visual signals** with arrows and text labels
- **Audio alerts** for H2/L2 setup formations (optional)

### Customizable Parameters
- **EMA Period**: Default 20, range 5-200
- **Swing Strength**: Default 5, range 1-10 (bars on each side for swing detection)
- **Show EMA**: Toggle EMA display
- **Show Swings**: Toggle swing high/low markers
- **Enable Alerts**: Toggle audio alerts

### Visual Indicators
- **Yellow EMA Line**: 20-bar exponential moving average
- **Cyan Dots**: Swing highs
- **Magenta Dots**: Swing lows
- **Green Up Arrows + "H2" Text**: H2 bull flag signals
- **Red Down Arrows + "L2" Text**: L2 bear flag signals

## Installation

### Method 1: Using NinjaTrader's Import Feature
1. Open NinjaTrader 8
2. Go to **Tools → Import → NinjaScript Add-On**
3. Select the `BrooksH2L2Pullback.cs` file
4. Click **Import**
5. Restart NinjaTrader if prompted

### Method 2: Manual Installation
1. Locate your NinjaTrader 8 documents folder (typically `Documents\NinjaTrader 8\`)
2. Navigate to `bin\Custom\Indicators\`
3. Copy `BrooksH2L2Pullback.cs` to this directory
4. In NinjaTrader, open the **NinjaScript Editor** (F11)
5. Click **Compile** (F5)
6. Verify compilation succeeds with no errors

## Usage

### Adding to a Chart
1. Open a chart in NinjaTrader 8
2. Right-click the chart and select **Indicators**
3. Find **BrooksH2L2Pullback** in the list
4. Click **Add** or double-click
5. Configure parameters as desired
6. Click **OK**

### Understanding the Signals

#### H2 Bull Flag Signal
When you see a **green up arrow** with "H2" text:
- The market is in a bull trend (price above rising EMA)
- A two-legged pullback has completed
- A strong bullish reversal bar has formed
- **Trading Action**: Consider entering long above the high of the signal bar
- **Stop Loss**: Below the recent swing low
- **Target**: Measure move from prior trend leg or use support/resistance

#### L2 Bear Flag Signal
When you see a **red down arrow** with "L2" text:
- The market is in a bear trend (price below falling EMA)
- A two-legged pullback has completed
- A strong bearish reversal bar has formed
- **Trading Action**: Consider entering short below the low of the signal bar
- **Stop Loss**: Above the recent swing high
- **Target**: Measure move from prior trend leg or use support/resistance

### Recommended Settings

#### For Intraday Trading (5-minute charts)
- EMA Period: 20
- Swing Strength: 3-5
- Enable Alerts: Yes

#### For Swing Trading (Daily charts)
- EMA Period: 20
- Swing Strength: 5-7
- Enable Alerts: Yes

#### For Scalping (1-3 minute charts)
- EMA Period: 20
- Swing Strength: 2-3
- Enable Alerts: Yes

## Trading Guidelines

### Best Practices
1. **Use in trending markets**: H2/L2 setups are trend continuation patterns - avoid in choppy, sideways markets
2. **Confirm with EMA**: Only take longs above EMA in bull trends, shorts below EMA in bear trends
3. **Signal bar quality matters**: Look for strong bullish (H2) or bearish (L2) bars with good body-to-wick ratios
4. **Context is key**: Consider larger timeframe trends and support/resistance levels
5. **Risk management**: Always use appropriate stop losses and position sizing

### What Makes a Good H2/L2 Setup
- **Clear trend** on the timeframe you're trading
- **Two distinct pullback legs** - not just minor wiggles
- **Pullback to or near the EMA** (but not too deep)
- **Strong signal bar** indicating resumption of trend
- **Volume confirmation** (if available) showing buyers (H2) or sellers (L2) stepping in

### Avoiding False Signals
- **Not every H2/L2 is high probability** - assess the overall context
- **Avoid in trading ranges** - these patterns need trending conditions
- **Watch for trend exhaustion** - multiple failed attempts may indicate trend end
- **Consider bar count** - very late in trends, these setups become riskier

## Technical Details

### Trend Identification
- **Bull Trend**: Price above EMA AND EMA rising (current EMA > previous EMA)
- **Bear Trend**: Price below EMA AND EMA falling (current EMA < previous EMA)

### Swing Detection
Uses NinjaTrader's built-in Swing indicator with configurable strength parameter:
- Swing high: High with specified number of lower highs on both sides
- Swing low: Low with specified number of higher lows on both sides

### Pullback Counting
- Tracks recent swing highs and lows
- Identifies two-legged pullback structures
- In bull trends: Looks for two swing lows below EMA (or approaching it)
- In bear trends: Looks for two swing highs above EMA (or approaching it)

### Signal Bar Criteria

**H2 Bull Signal Bar**:
- Close > Open (bullish bar)
- Close near the high (strong close): `(Close - Low) > (High - Close) * 0.5`
- Moving upward: `Close > previous Close` and `Close > previous Low`

**L2 Bear Signal Bar**:
- Close < Open (bearish bar)
- Close near the low (strong close): `(High - Close) > (Close - Low) * 0.5`
- Moving downward: `Close < previous Close` and `Close < previous High`

## Limitations and Disclaimers

1. **Lagging by nature**: Swing detection requires confirmation bars, so signals are not predictive
2. **Not a complete trading system**: This indicator identifies setups but doesn't provide complete entry/exit rules
3. **Requires interpretation**: Price action trading requires skill and experience
4. **No guarantee of profits**: Past performance does not indicate future results
5. **Use with risk management**: Always use appropriate stops and position sizing

## Resources

### Al Brooks Educational Materials
- [Brooks Trading Course](https://www.brookstradingcourse.com/)
- [10 Best Price Action Trading Patterns](https://www.brookstradingcourse.com/price-action/10-best-price-action-trading-patterns/)
- Al Brooks Books: "Reading Price Charts Bar by Bar" series

### Recommended Complementary Indicators
- Volume indicators for confirmation
- Support/Resistance levels
- ATR for stop loss placement
- Higher timeframe trend indicators

## Support and Customization

This indicator is designed to be a solid implementation of Al Brooks' H2/L2 methodology. However, every trader may have slight variations in how they interpret these patterns.

### Customization Ideas
- Adjust EMA period for different market conditions
- Modify swing strength for different timeframes
- Add additional filters (volume, ATR, etc.)
- Integrate with other Brooks patterns (wedges, trading range reversals, etc.)
- Add automatic entry orders or strategy conversion

## Version History

### Version 1.0
- Initial release
- Core H2/L2 pattern detection
- 20-bar EMA trend filter
- Configurable swing detection
- Visual signals and alerts
- Full parameter customization

## License

This indicator is provided as-is for educational and trading purposes.

## Credits

Based on the price action trading methodology developed by Al Brooks.
Implemented for NinjaTrader 8 platform.

---

**Disclaimer**: Trading futures, forex, and options involves substantial risk of loss and is not suitable for all investors. Past performance is not necessarily indicative of future results. Use this indicator as part of a complete trading plan with appropriate risk management.
