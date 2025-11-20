# Brooks H2/L2 Pullback Strategy - Automated Trading

## Overview

This is an **automated trading strategy** for NinjaTrader 8 that automatically places orders based on Al Brooks' H2 (High 2) and L2 (Low 2) pullback patterns. Unlike the indicator version which only signals patterns, this strategy **automatically enters trades and places stop loss orders**.

## Entry and Exit Rules (Per Al Brooks Methodology)

### H2 Bull Flag (Long Trades)

**Entry:**
- Buy stop order placed **1 tick above the high of the H2 signal bar**
- Order is triggered when price breaks above this level

**Stop Loss:**
- Placed **1 tick below the low of the H2 signal bar**
- Protects against pattern failure

**Example:**
```
H2 Signal Bar:
  High: 4510.00
  Low: 4507.50

Orders Placed:
  Entry: Buy stop at 4510.25 (1 tick above high)
  Stop:  Stop loss at 4507.25 (1 tick below low)
  Risk:  3.00 points
```

### L2 Bear Flag (Short Trades)

**Entry:**
- Sell stop order placed **1 tick below the low of the L2 signal bar**
- Order is triggered when price breaks below this level

**Stop Loss:**
- Placed **1 tick above the high of the L2 signal bar**
- Protects against pattern failure

**Example:**
```
L2 Signal Bar:
  High: 15471.00
  Low: 15465.50

Orders Placed:
  Entry: Sell stop at 15465.25 (1 tick below low)
  Stop:  Stop loss at 15471.25 (1 tick above high)
  Risk:  6.00 points
```

## Key Differences from Indicator

| Feature | Indicator | Strategy |
|---------|-----------|----------|
| Shows signals | ✅ Yes | ✅ Yes |
| Places entry orders | ❌ No | ✅ Yes (automatically) |
| Places stop orders | ❌ No | ✅ Yes (automatically) |
| Manages positions | ❌ No | ✅ Yes |
| Risk per trade | N/A | Defined by signal bar size |
| Requires manual trading | ✅ Yes | ❌ No (fully automated) |

## Installation

### Method 1: Manual Installation

1. **Locate your NinjaTrader 8 Documents folder:**
   ```
   Documents\NinjaTrader 8\bin\Custom\Strategies\
   ```

2. **Copy the strategy file:**
   - Copy `BrooksH2L2Strategy.cs` to the Strategies folder

3. **Compile:**
   - Open NinjaTrader 8
   - Press F11 (NinjaScript Editor)
   - Press F5 (Compile)
   - Verify successful compilation

4. **Verify installation:**
   - Open any chart
   - Click "Strategies" button
   - Look for "BrooksH2L2Strategy" in the list

## Configuration

### Adding to Chart

1. Open a chart in NinjaTrader 8
2. Click the **Strategies** button (or right-click → Strategies)
3. Find **BrooksH2L2Strategy** in the list
4. Double-click or click "Add"
5. Configure parameters (see below)
6. Click "OK"

### Parameters

#### Required Parameters

**EMA Period** (Default: 20)
- Period for the trend filter EMA
- Range: 5-200
- Al Brooks standard: 20

**Swing Strength** (Default: 5)
- Number of bars on each side for swing detection
- Range: 1-10
- Higher = fewer, cleaner signals
- Lower = more signals, potentially noisier

**Quantity** (Default: 1)
- Number of contracts to trade per signal
- Range: 1 to unlimited
- Start with 1 contract while learning

**Enable Alerts** (Default: Yes)
- Audio alerts when signals appear
- Helpful for monitoring

#### Visual Settings

**H2 Signal Color** (Default: Lime/Green)
- Color for H2 bull flag markers

**L2 Signal Color** (Default: Red)
- Color for L2 bear flag markers

### Strategy Settings

When adding the strategy to a chart, also configure these NinjaTrader strategy settings:

**Account Settings:**
- Select your trading account (Sim101 for simulation)
- Set account size for proper risk management

**Order Properties:**
- Order Type: Market (for stop orders)
- Time in Force: GTC (Good Till Cancelled)
- Enable "Wait until flat" for safety

**Position:**
- Entries Per Direction: 1 (one position at a time)
- Stop & Target: Strategy handles stops

## Usage

### Step 1: Start with Simulation

**IMPORTANT**: Always test on Sim account first!

1. Enable strategy on Sim account chart
2. Set Quantity to 1
3. Monitor for several days
4. Review performance and behavior

### Step 2: Monitor Strategy Execution

Watch for:
- Green arrows with "H2" = Long entry pending
- Red arrows with "L2" = Short entry pending
- Order confirmations in NinjaTrader output window
- Position and stop loss placement

### Step 3: Strategy Behavior

**When H2 Signal Appears:**
```
1. Strategy identifies H2 pattern
2. Draws green up arrow + "H2" text
3. Places buy stop order 1 tick above signal bar high
4. Sets stop loss order 1 tick below signal bar low
5. If price breaks above high → entry fills
6. If stop triggered → loss limited to signal bar range
```

**When L2 Signal Appears:**
```
1. Strategy identifies L2 pattern
2. Draws red down arrow + "L2" text
3. Places sell stop order 1 tick below signal bar low
4. Sets stop loss order 1 tick above signal bar high
5. If price breaks below low → entry fills
6. If stop triggered → loss limited to signal bar range
```

### Step 4: Position Management

**Entry Rules:**
- Only 1 position at a time (EntriesPerDirection = 1)
- Must be flat before next signal
- No pyramiding or scaling in

**Stop Loss:**
- Automatically placed with entry
- Based on signal bar high/low
- Never manually move stop closer (against you)

**Exit:**
- Strategy currently uses only stop loss
- You can add profit targets manually
- Or let runners work with trailing stops

## Risk Management

### Understanding Risk Per Trade

Each trade's risk is determined by the signal bar size:

**H2 Long Trade Risk:**
```
Risk = (Signal Bar High + 1 tick) - (Signal Bar Low - 1 tick)
     = Signal Bar Range + 2 ticks
```

**L2 Short Trade Risk:**
```
Risk = (Signal Bar High + 1 tick) - (Signal Bar Low - 1 tick)
     = Signal Bar Range + 2 ticks
```

### Position Sizing

**Conservative (1% risk per trade):**
```
Account: $50,000
Risk per trade: 1% = $500

If signal bar = 3 points (ES @ $12.50/tick, 12 ticks):
Risk per contract = 12 ticks × $12.50 = $150
Position size = $500 ÷ $150 = 3.3 → 3 contracts

Set Quantity = 3
```

**Aggressive (2% risk per trade):**
```
Account: $50,000
Risk per trade: 2% = $1,000

Same signal:
Position size = $1,000 ÷ $150 = 6.6 → 6 contracts

Set Quantity = 6
```

**Note:** Signal bar sizes vary, so risk per trade will vary. Consider using consistent position size until you're experienced.

### Daily Loss Limits

Set maximum daily loss in NinjaTrader:
1. Strategy → Properties
2. "Maximum number of losing days" or use account-level controls
3. Recommended: Stop after 3 consecutive losses

## Performance Expectations

### Realistic Statistics

Based on Al Brooks methodology:

**Strong Trending Markets:**
- Win Rate: 60-70%
- Average Risk/Reward: 1:2 to 1:3
- These are your best conditions

**Moderate Trends:**
- Win Rate: 50-60%
- Average Risk/Reward: 1:1.5 to 1:2
- Acceptable trading

**Choppy/Range Markets:**
- Win Rate: 30-40%
- Average Risk/Reward: Poor
- **Strategy will generate losses** - avoid these conditions

### Important Notes

- Not every signal will be high quality
- The strategy doesn't know market context (news, major levels, etc.)
- You should monitor and can disable strategy in poor conditions
- Backtesting results may differ from live due to slippage and fills

## Monitoring the Strategy

### What to Watch

**Execution Window:**
- Entry order placements
- Entry fills
- Stop loss placements
- Stop hits

**Chart:**
- Visual H2/L2 markers
- Trend quality (price vs EMA)
- Signal bar quality

**Account:**
- P&L per trade
- Win rate
- Maximum drawdown

### When to Disable Strategy

❌ **Disable during:**
- Major news events (FOMC, NFP, etc.)
- Market open first 15 minutes (choppy)
- Low volume sessions (holidays)
- When trend is unclear or choppy
- After hitting daily loss limit

✅ **Enable during:**
- Clear trending markets
- Normal volume sessions
- Your best trading hours
- When you can monitor

## Troubleshooting

### No Trades Executing

**Check:**
1. Is account selected and connected?
2. Is "Enabled" checkbox on?
3. Is market in a clear trend?
4. Are there H2/L2 signals appearing?
5. Check NinjaTrader output for errors

### Orders Not Filling

**Possible Causes:**
1. Price didn't reach stop order level
2. Slippage on fast markets
3. Insufficient buying/selling power
4. Check order type settings

### Unexpected Losses

**Review:**
1. Were signals in choppy markets?
2. Signal bar quality (weak bars = lower probability)
3. Trend context (late trend signals = riskier)
4. News events causing volatility

### Strategy Stops Working

**Steps:**
1. Check NinjaTrader connection
2. Verify account has funds
3. Check for errors in Log tab
4. Recompile strategy (F5)
5. Remove and re-add to chart

## Advanced Features

### Customization Ideas

You can modify the strategy code to add:

**Profit Targets:**
```csharp
// Add after entry
SetProfitTarget("H2Long", CalculationMode.Ticks, 20);  // 20 tick target
```

**Trailing Stops:**
```csharp
// Add in OnBarUpdate after entry
SetTrailStop("H2Long", CalculationMode.Ticks, 10, false);
```

**Time Filters:**
```csharp
// Only trade during specific hours
if (ToTime(Time[0]) >= 093000 && ToTime(Time[0]) <= 160000)
{
    // Pattern detection code
}
```

**Maximum Daily Trades:**
```csharp
// Add variable: private int tradesThisDay = 0;
// Add check: if (tradesThisDay >= 5) return;
```

### Strategy Analyzer

Use NinjaTrader's Strategy Analyzer to:
- Backtest on historical data
- Optimize parameters (EMA period, swing strength)
- Analyze win rate, profit factor, max drawdown
- Test different instruments and timeframes

**Recommended Backtesting:**
- Minimum 6 months of data
- Include commission and slippage
- Multiple market conditions
- Different timeframes

## Comparison: Indicator vs Strategy

### When to Use the Indicator

✅ Use indicator if you:
- Want to learn the patterns first
- Prefer manual trade execution
- Want to apply your own filters
- Need to check other confluences
- Are still developing your approach

### When to Use the Strategy

✅ Use strategy if you:
- Understand H2/L2 patterns well
- Want fully automated execution
- Can monitor but don't want to click orders
- Trust the Al Brooks methodology
- Have tested thoroughly on Sim

### Using Both Together

Many traders:
1. Use indicator on main trading charts
2. Use strategy on separate chart in Sim
3. Compare manual vs automated results
4. Build confidence over time
5. Transition to strategy when comfortable

## Best Practices

### Getting Started

1. **Education First**
   - Read Al Brooks materials
   - Study the indicator signals
   - Understand H2/L2 theory

2. **Simulation Testing**
   - Run strategy on Sim for 1-2 weeks minimum
   - Start with 1 contract
   - Monitor all executions
   - Keep a journal

3. **Live Trading**
   - Start with smallest size (1 contract)
   - Trade only best market conditions
   - Set strict daily loss limits
   - Gradually increase size

### Ongoing Management

- Review trades daily
- Track win rate and R:R
- Note best/worst conditions
- Adjust parameters if needed
- Take breaks after losses

## Safety Features

The strategy includes:

✅ **Position Limits**
- Maximum 1 position per direction
- Must be flat before new entry

✅ **Stop Loss**
- Always placed with entry
- Based on signal bar structure
- Protects every trade

✅ **Session Close**
- Exits on session close (configurable)
- Prevents overnight gaps

✅ **Flat Start**
- Waits until flat before starting
- Prevents inheriting positions

## Warnings and Disclaimers

⚠️ **Important:**
- Automated trading carries significant risk
- Past performance doesn't guarantee future results
- Always start with simulation
- Never risk more than you can afford to lose
- Monitor the strategy regularly
- Market conditions change

⚠️ **Strategy Limitations:**
- Cannot judge overall market context
- Doesn't know about major news events
- Doesn't adjust for support/resistance
- Fixed position size (you set Quantity)
- No profit target (only stop loss)

⚠️ **User Responsibility:**
- You must set appropriate Quantity
- You must monitor market conditions
- You must disable during news/chop
- You are responsible for all trades
- This is a tool, not a guarantee

## Support Files

- **Indicator Version:** `Indicators/BrooksH2L2Pullback.cs` (for visual analysis)
- **Pattern Guide:** `PATTERNS.md` (learn the setups)
- **Trading Examples:** `EXAMPLES.md` (see real scenarios)
- **Quick Reference:** `QUICK_REFERENCE.md` (cheat sheet)

## Summary

The BrooksH2L2Strategy automates Al Brooks' H2/L2 pullback pattern trading:

✅ **Automatic entry** 1 tick beyond signal bar
✅ **Automatic stop** 1 tick beyond opposite side
✅ **Risk defined** by signal bar size
✅ **One position** at a time
✅ **Visual markers** show signals
✅ **Full automation** when enabled

**Remember:** Start with Sim, test thoroughly, use proper position sizing, and always monitor your automated strategies!

---

**Questions?** Review the main documentation files or Al Brooks trading course materials.

**Ready to Trade?** Start on Sim account and track your results!
