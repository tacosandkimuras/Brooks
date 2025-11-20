# Brooks Price Action Trading Tools

This repository contains NinjaTrader 8 indicators and strategies implementing Al Brooks' price action trading methodology.

## Tools Included

### 1. BrooksH2L2Pullback - H2/L2 Pullback Indicator (Visual Signals Only)

An indicator that identifies **H2 (High 2) Bull Flag** and **L2 (Low 2) Bear Flag** patterns - two of Al Brooks' top 10 best price action trading patterns.

**Key Features:**
- Automatic H2 bull flag detection in uptrends
- Automatic L2 bear flag detection in downtrends
- 20-bar EMA trend filter (configurable)
- Visual signals with arrows and text
- Configurable swing detection
- Audio alerts for trade setups
- **Manual trading** - you place orders yourself

### 2. BrooksH2L2Strategy - H2/L2 Pullback Strategy (AUTOMATED TRADING)

An automated trading strategy that **automatically places orders and stops** based on H2/L2 patterns.

**Key Features:**
- **Automatic order placement** - 1 tick above H2 high (long) or 1 tick below L2 low (short)
- **Automatic stop loss** - 1 tick below H2 low (long) or 1 tick above L2 high (short)
- Same pattern detection as indicator
- Full position management
- Configurable position size
- Per Al Brooks' entry and stop specifications

**What are H2/L2 Patterns?**
- **H2 (High 2)**: Second entry long setup after a two-legged pullback in a bull trend
- **L2 (Low 2)**: Second entry short setup after a two-legged pullback in a bear trend

These are high-probability trend continuation patterns that wait for the second attempt to resume the trend, avoiding the common trap of entering on the first pullback.

## Which Tool Should I Use?

| Use Case | Tool | Why |
|----------|------|-----|
| Learning patterns | **Indicator** | Visual signals, you control entries |
| Discretionary trading | **Indicator** | Apply your own filters and judgment |
| Fully automated trading | **Strategy** | Hands-off execution with stops |
| Backtesting | **Strategy** | Test historical performance |
| You want to verify setups | **Indicator** | Manual confirmation before entry |
| You trust the methodology | **Strategy** | Automatic execution per Brooks rules |

## Installation

### Indicator Installation
See the detailed [Indicators README](./Indicators/README.md) for complete installation and usage instructions.

**Quick Install:**
1. Copy `Indicators/BrooksH2L2Pullback.cs` to your NinjaTrader 8 `Documents\NinjaTrader 8\bin\Custom\Indicators\` folder
2. Compile in NinjaScript Editor (F5)
3. Add to chart via Indicators menu

### Strategy Installation
See the detailed [Strategies README](./Strategies/README.md) for complete installation and usage instructions.

**Quick Install:**
1. Copy `Strategies/BrooksH2L2Strategy.cs` to your NinjaTrader 8 `Documents\NinjaTrader 8\bin\Custom\Strategies\` folder
2. Compile in NinjaScript Editor (F5)
3. Add to chart via Strategies button
4. **⚠️ START WITH SIM ACCOUNT!**

## Usage

### Using the Indicator (Manual Trading)
1. Add the BrooksH2L2Pullback indicator to your chart
2. Configure the EMA period (default: 20) and swing strength (default: 5)
3. Look for:
   - **Green up arrows with "H2"** = Long entry signals in bull trends
   - **Red down arrows with "L2"** = Short entry signals in bear trends
4. **You manually place orders** - buy stop 1 tick above H2 high, sell stop 1 tick below L2 low
5. **You manually place stops** - below H2 low (long) or above L2 high (short)

### Using the Strategy (Automated Trading)
1. Add the BrooksH2L2Strategy to your chart
2. Configure the EMA period (default: 20), swing strength (default: 5), and quantity
3. **Strategy automatically:**
   - Places buy stop 1 tick above H2 signal bar high
   - Places sell stop 1 tick below L2 signal bar low
   - Sets stop loss 1 tick below H2 low (long) or 1 tick above L2 high (short)
   - Manages the entire position
4. **⚠️ ALWAYS test on Sim account first!**

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
