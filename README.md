# Project: Backtesting of Systematic Hedging Strategy

## **Objective**

This project involves implementing a financial application using Microsoft's **.NET** framework, designed as a decision-making tool to perform validation tests—**Backtests** and **Forward tests**—on hedging portfolios of **Basket Options**.

The software, developed in **C#**, evaluates the performance of a portfolio of assets using available market data. The main components of this project include:

### - **BacktestConsole:**

The **BacktestConsole** performs the backtests, providing the hedging portfolio prices at selected discretization times using a dynamic rebalancing strategy. This rebalancing ensures that the portfolio composition is updated at each step with new quantities (**deltas**) for each asset to best match the replicated portfolio values.

- **Deltas**: Sensitivities of the option price to movements in the underlying assets. These deltas are extracted from **TestData**.

#### **Usage:**

```bash
BacktestConsole.exe <test-params> <mkt-data> <output-file>
```

### - **gRPCServer and BacktestEvaluation:**
The **gRPC** tool allows client(**BacktestEvaluation**) - server (**BacktestServer**) communication. The client invokes the server to computate the portfolio replicating data for given test parameters and market data. The server returns a message with the computation results, and the client is invoked by the user to get the output values (prices).

