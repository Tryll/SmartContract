namespace SmartApps {
        
    // Tag's a function to exposed on the exposure layer, be it Rest or what ever.
    // After the function is executed a Commit operation will be done to save all changes.
    // If this fails the "transaction" will be "rolled back".
    public class ExposedFunctionAttribute {

    }


    // Identifies attributes to be persistently stored by the Persistency Layer
    // All other properties will be ignored by the layer
    public class ConsistentPropertyAttribute {

    }



    // Required properties by all Distributed Apps / Smart Contracts
    public interface ISmartContract {

        [ConsistentProperty]
        public string identifier;
        [ConsistentProperty]
        public int revision = 0;
    }


    public abstract class SmartContract<T> : ISmartContract  where T:ISmartContract  {

        public IPersistentSmartContract<T> peristencyLayer;

        // invoked by dependency injection
        public SmartContract(IPersistentSmartContract<T> persistencyLayer) {
            this.peristencyLayer=persistencyLayer;
        }


        // This will be called by the MicoService SmartContract Servicer
        // All exceptions will be thrown up to it.
        public void Commit() {
            var postCommitVersion = peristencyLayer.TryGet(this.identifier);
            if (postCommitVersion.revision!=this.revision) throw ("Stale object.");
            
            Validator.AttributeValidation(this);
            PreCommitVerification(postCommitVersion);
            persistencyLayer.TryCommitTransaction(postCommitVersion, this);
        }

        // this is a function that can do complex validation on multiple properties or call external resources.
        // this is the final verificaiton done by the smart contract before commit is attempted.
        public virtual bool PreCommitVerification(Party postCommitRevision) {}
        
    }


    // The persistency layer has to implement this
    // The persistency layer will be registered and used by dependency injection
    // It can persist the values on Lock based SQL, ledgers / chains, multiple distributed instances and so on.
    // Any consensus algorithm can be used based on service requirements.
    public interface IPersistentSmartContract<T>  where T:ISmartContract  {

        // self explanatory
        T IPersistentSmartContract.TryGet(string identifier);


        // Take all ConsistentProperties and identifies what has changed
        // OnChange events cannot be raised as they cannot be canceled and they cannot be sure transaciton actually can complete,
        // OnChanged events will be raised after transaction is confirmed.
        // On commit revision must be checked again and increased.
        // If no changes are required it will not do anything.
        void IPersistentSmartContract.TryCommitTransaction(T oldRevision, T newRevision);
    }

}