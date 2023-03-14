
using System.ComponentModel.DataAnnotations;
using SmartApps;

// Distributed app with strong data consistency
// It can run on any distributed microservice with any persistency layer based on any consensus model
public class Party : SmartContract<Party> {

    [MinLength(1,"Name not long enough")]
    [NotNullOrEmpty]
    [ConsistentProperty]
    public string Name { get; set; }    

    [NotNullOrEmpty]
    [ConsistentProperty]
    public string Address { get; set; }    

    [NotNullOrEmpty]
    [ConsistentProperty]
    public string ZipCode { get; set {

            if ( Int32.Parse(ZipCode)!=0) throw("ZipCode has to be a numeric string");
            this.value = value;

        }
    } 

    [EmailAddress]
    [ConsistentProperty]
    public string Email { get; set; }


    [Range(18, 200)]
     [ConsistentProperty]
    public int Age { get; set; }


    [ExposedFunction]
    public string GetEmoticon() {
        return $"{this.Name} says :)";
    }   

    [ExposedFunction]
    public string NewNameByMagic(string nameSeed) { 
        Random rnd = new Random();
        return this.Name= nameSeed.ToCharArray().OrderBy(x => rnd.Next()).Take(10).ToString();
    }   

    // Last verification step before commit is attempted.
    // this can capture complex business logic
    public override bool PreCommitVerification(Party postCommitRevision) {
        if (Name == Address) throw("Name and address cannot be the same");

    }

}




