using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backEnd.Model{

    public class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id{get;set;}
        public string name{get;set;}
        public string type{ get; set; }
        public string vendor{get;set;}
    	public string product{get;set;}
    	public string version{get;set;}
        public string date{get;set;}

    }

    
}