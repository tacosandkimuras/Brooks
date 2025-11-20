# H2 and L2 Pattern Examples

## H2 Bull Flag Pattern (Long Setup)

```
Price Action Chart - Uptrend with H2 Setup

    │                                          ┌──────┐
    │                                          │      │ New High
    │                                          │      │ (Target)
    │                        ┌────┐           │      │
    │                        │    │           │      │
    │                        │    │   ┌──H2 Signal (Entry here)
    │                       ┌┘    │   │      ▲ 
    │                       │     └───┘   Green Arrow
    │      ┌────┐          │
    │      │    │          │  Leg 2 Down
    │     ┌┘    │          │  (2nd pullback leg)
─EMA─────┘─────└─────────┘
    │                    │
    │                    └── Leg 1 Down  
    │                        (1st pullback leg)
    │   Initial Uptrend
    │
    └──────────────────────────────────────────────────▶ Time

Key Elements:
1. Price above rising 20 EMA (Bull Trend)
2. Two-legged pullback (Leg 1 and Leg 2 down)
3. Pullback typically reaches or approaches EMA
4. H2: Strong bullish bar after 2nd leg (green arrow + "H2" label)
5. Entry: Buy stop above H2 signal bar high
6. Stop: Below recent swing low
```

## L2 Bear Flag Pattern (Short Setup)

```
Price Action Chart - Downtrend with L2 Setup

    │   Initial Downtrend
    │
    │                    ┌── Leg 1 Up
    │                    │   (1st pullback leg)
─EMA─────┐─────┌─────────┐
    │    │     │         │  Leg 2 Up
    │    └┐    │         │  (2nd pullback leg)
    │     │    └────┐    │
    │     └────┘    │    │
    │               │    └────┐
    │               │         │   ┌──L2 Signal (Entry here)
    │               │         │   │      ▼
    │               │         └───┘   Red Arrow
    │               │           │      │
    │               │           │      │
    │               │           └──────┘
    │               │                  New Low
    │               │                  (Target)
    │               │
    └───────────────────────────────────────────────────▶ Time

Key Elements:
1. Price below falling 20 EMA (Bear Trend)
2. Two-legged pullback (Leg 1 and Leg 2 up)
3. Pullback typically reaches or approaches EMA
4. L2: Strong bearish bar after 2nd leg (red arrow + "L2" label)
5. Entry: Sell stop below L2 signal bar low
6. Stop: Above recent swing high
```

## Signal Bar Quality

### Good H2 Bull Signal Bar
```
High ──┐
       │ Small upper wick
Close ─┤█████ Strong bullish body
       │█████ (close near high)
Open ──┤█████
       │ Larger lower wick OK
Low ───┘

Criteria:
✓ Close > Open (bullish)
✓ Close near high: (Close - Low) > (High - Close) × 0.5
✓ Shows buying pressure
```

### Good L2 Bear Signal Bar
```
High ──┐ Larger upper wick OK
       │
Open ──┤█████
       │█████ Strong bearish body
Close ─┤█████ (close near low)
       │ Small lower wick
Low ───┘

Criteria:
✓ Close < Open (bearish)
✓ Close near low: (High - Close) > (Close - Low) × 0.5
✓ Shows selling pressure
```

## Pullback Leg Identification

### What Counts as a Leg?

```
A clear pullback leg has:
1. Defined swing high/low
2. Multiple bars in the direction
3. Visible on the chart (not just 1-2 bar wiggle)

Good Two-Legged Pullback (H2 setup):
        ┌─────┐         Swing High 1
        │     │
        │     └───┐     Swing High 2
        │         │
─────────┘        └─── Clear Leg 1 and Leg 2

Poor/Unclear Structure:
        ┌┐┌┐┌──┐  Too choppy
        ││││  │  Hard to count legs
──────┘└┘└┘  └─ Avoid these setups
```

## Complete Trading Setup Example

### H2 Bull Flag Trade Plan

```
Entry Point:
Buy stop at: Signal bar high + 1 tick

Stop Loss:
Below: Most recent swing low
Risk: Entry - Stop = X ticks

Target Options:
1. Measured Move: Equal to prior trend leg
2. Prior swing high
3. 2× Risk (2:1 reward)

Position Size:
Based on risk (X ticks) and account rules


Chart Example:
                    Target ┌────────
                          ┌┘        
               ┌────┐    │  
               │    └────┘ Entry ← Buy stop here
              ┌┘       ▲  
─────────────┘       H2  
     │              
     └─ Stop (below swing low)
```

### L2 Bear Flag Trade Plan

```
Entry Point:
Sell stop at: Signal bar low - 1 tick

Stop Loss:
Above: Most recent swing high
Risk: Stop - Entry = X ticks

Target Options:
1. Measured Move: Equal to prior trend leg
2. Prior swing low
3. 2× Risk (2:1 reward)

Position Size:
Based on risk (X ticks) and account rules


Chart Example:

     ┌─ Stop (above swing high)
     │
─────────────┐       
             │       L2  
             └┐       ▼
              │    ┌────┐ Entry ← Sell stop here  
              └────┘    │
                   └────┘
                        │
                        └──────── Target
```

## Trend Context

### Bull Trend Characteristics
```
✓ Price consistently above 20 EMA
✓ EMA rising (EMA[0] > EMA[1])
✓ Higher highs and higher lows
✓ Strong bullish bars dominating
✓ Pullbacks find support at/near EMA

Good for H2 setups
```

### Bear Trend Characteristics
```
✓ Price consistently below 20 EMA
✓ EMA falling (EMA[0] < EMA[1])
✓ Lower highs and lower lows
✓ Strong bearish bars dominating
✓ Pullbacks find resistance at/near EMA

Good for L2 setups
```

### Avoid in Range/Chop
```
Price oscillating around EMA
No clear trend direction
✗ NOT suitable for H2/L2 trading
Look for breakout patterns instead
```

## Common Mistakes to Avoid

1. **Taking H1/L1 (first entry)**
   - Wait for the second leg
   - First entries have lower probability

2. **Ignoring trend**
   - Don't take H2 in downtrends
   - Don't take L2 in uptrends

3. **Poor signal bars**
   - Weak bars (small, doji-like) are lower probability
   - Need strong directional bars

4. **Late in trend**
   - After many legs, trend may be exhausted
   - Be more selective with late-trend H2/L2

5. **Fighting major levels**
   - Don't take H2 into major resistance
   - Don't take L2 into major support

## Indicator Signals Summary

### What You'll See on Chart

**Cyan Dots**: Swing highs (peaks)
**Magenta Dots**: Swing lows (valleys)
**Yellow Line**: 20 EMA (trend filter)
**Green Arrow ↑ + "H2"**: H2 bull flag long setup
**Red Arrow ↓ + "L2"**: L2 bear flag short setup

### When Signals Appear

- Signals appear when all conditions are met:
  1. Clear trend (relative to EMA)
  2. Two pullback legs detected
  3. Strong signal bar in trend direction
  4. Bar moving back toward trend

- Not every pullback generates a signal
- Absence of signal doesn't mean no trading opportunity
  (Use your own analysis too!)

## Practice Exercise

Study your charts and identify:
1. Can you spot the 20 EMA?
2. Is price in a trend (above/below EMA)?
3. Count the pullback legs (1, 2, or more)
4. Note when strong reversal bars appear
5. Would you take this trade based on context?

Start with historical data, then move to real-time observation before live trading.

---

**Remember**: The indicator identifies potential setups. You still need to evaluate:
- Overall market context
- Support/Resistance levels
- Risk/Reward ratio
- Your trading plan rules
