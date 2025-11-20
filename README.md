# Brooks Price Action Trading Tools

This repository contains NinjaTrader 8 indicators implementing Al Brooks' price action trading methodology.

## Indicators

### BrooksH2L2Pullback - H2/L2 Pullback Indicator

An automated indicator that identifies **H2 (High 2) Bull Flag** and **L2 (Low 2) Bear Flag** patterns - two of Al Brooks' top 10 best price action trading patterns.

**Key Features:**
- Automatic H2 bull flag detection in uptrends
- Automatic L2 bear flag detection in downtrends
- 20-bar EMA trend filter (configurable)
- Visual signals with arrows and text
- Configurable swing detection
- Audio alerts for trade setups

**What are H2/L2 Patterns?**
- **H2 (High 2)**: Second entry long setup after a two-legged pullback in a bull trend
- **L2 (Low 2)**: Second entry short setup after a two-legged pullback in a bear trend

These are high-probability trend continuation patterns that wait for the second attempt to resume the trend, avoiding the common trap of entering on the first pullback.

## Installation

See the detailed [Indicators README](./Indicators/README.md) for complete installation and usage instructions.

**Quick Install:**
1. Copy `Indicators/BrooksH2L2Pullback.cs` to your NinjaTrader 8 `Documents\NinjaTrader 8\bin\Custom\Indicators\` folder
2. Compile in NinjaScript Editor (F5)
3. Add to chart via Indicators menu

## Usage

1. Add the BrooksH2L2Pullback indicator to your chart
2. Configure the EMA period (default: 20) and swing strength (default: 5)
3. Look for:
   - **Green up arrows with "H2"** = Long entry signals in bull trends
   - **Red down arrows with "L2"** = Short entry signals in bear trends

## Trading Guidelines

- **Best used in trending markets** - avoid choppy, sideways price action
- **Confirm with EMA** - take longs above EMA, shorts below EMA
- **Use proper risk management** - set stops below swing lows (H2) or above swing highs (L2)
- **Consider context** - not every H2/L2 is high probability; evaluate trend strength and location

## Resources

- [Al Brooks Trading Course](https://www.brookstradingcourse.com/)
- [10 Best Price Action Trading Patterns](https://www.brookstradingcourse.com/price-action/10-best-price-action-trading-patterns/)
- Al Brooks Books: "Reading Price Charts Bar by Bar" series

## License

Educational and trading use. See indicator files for details.

## Disclaimer

Trading involves substantial risk of loss. This indicator is provided for educational purposes. Past performance does not guarantee future results. Always use appropriate risk management.
