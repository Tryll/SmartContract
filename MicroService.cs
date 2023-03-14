

// Psuedocode to illustrate usage:
//
// Dependency injecting the persistency layer based on Smart App / micro service representation requirements
//

// Classic  LockBasedSQL
var webBuilder = new RestWebBuilder();
webBuilder.Service<IChangeNotifer, OnChangeAndChanged>();
webBuilder.Service<IPersistentSmartContract<>, LockBasedSQL<>>();
webBuilder.HostSmartApp(Party);


// Hoster exposes all properties as 
// GET => PersistencyLayer.Get(id).Property
// SET => GET + SetValue + Commit on SmartApp to trigger business logic check
//
// Functions are exposed as Run() + Commit() to capture changes
//
// Exceptions are thrown back to requester. 