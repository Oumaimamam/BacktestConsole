# Project : Backtesting of Systematic hedging strategy :

**1. Objective:**
Implementing a financial application using Microsoft **.NET** framework serving as a decision tool to make validation tests (Backtests and Forward tests) on a Basket Option hedging portfolios. 
The software is developped in **C#** and is able to evaluate the performance of a portfolio of assets using market available data. 
The **BacktestConsole** performs the backtest and outputs the hedging portfolio prices at the chosen discretization times using a dynamic rebalancing strategy. The rebalancing means that the portfolio composition is updated at each steps with new quantities (deltas) of each asset to match at best the replicated portfolio values. Deltas here are sensitivities of the options price to the moves in the underlyings. They're extracted from the **TestData**. 
To use it : 
'''bash
BacktestConsole.exe <test-params> <mkt-data> <output-file>
'''
The **gRPC** tool allows client(**BacktestEvaluation**) - server (**BacktestServer**) communication. The client invokes the server to computate the portfolio replicating data for given test parameters and market data. The server returns a message with the computation results, and the client is invoked by the user to get the output values (prices).

