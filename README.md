# NordPoolC
NordPool connect by .net framework4.5

Due to the lack of username and password, the project has not yet been completed

Catalog Description：
CaoNC.PresentationFramework ：Extract and downgrade some features from the official libraries of. NET Framework 4.6 and above to. NET Framework 4.5.2
NordPoolC ：Born from public-intraday-api-net-example-main，NordPool Connection Library
NordPoolCSample：Sample，current Have no function

NordPoolC Use Method:
LogFactory.RegisterDebugLog,LogFactory.RegisterWarningLog,LogFactory.RegisterInfoLog,LogFactory.RegisterErrorLog,LogFactory.RegisterExceptionLog
->INPool.ConfigIniFile(NordPoolCSample\bin\Debug\config.ini);
->INPool.CreateSubscribeRequestBuilder
->INPool.CreateMarketDataClient,NPool.CreateTradeClient
->IClient.SubscribeDeliveryAreasAsync //marketData
->IClient.SubscribeConfigurationsAsync //trade
->IClient.SubscribeOrderExecutionReportAsync  //trade
->IClient.SubscribeContractsAsync //marketData
->IClient.SubscribeLocalViewsAsync //marketData
->IClient.SubscribePrivateTradesAsync //trade
->IClient.SubscribeTickersAsync //marketData
->IClient.SubscribeMyTickersAsync //marketData
->IClient.SubscribePublicStatisticsAsync //marketData
->IClient.SubscribeThrottlingLimitsAsync //trade
->IClient.SubscribeCapacitiesAsync //marketData
->trading
......
->IClient.DisconnectAsync