

// Psuedocode to illustrate usage:
//
// Dependency injecting the persistency layer based on Smart App / micro service representation requirements
//

// Classic  LockBasedSQL
var webBuilder = new RestWebBuilder();
webBuilder.Service<IChangeNotifer, OnChangeAndChanged>();
webBuilder.Service<IPersistentSmartContract<>, LockBasedSQL<>>();
webBuilder.HostSmartApp(Party);


