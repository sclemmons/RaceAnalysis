using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nest;

namespace RaceAnalysis.Models
{
    [ElasticsearchType]
    public class RequestContext
    {
        public RequestContext() { }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestContextId { get; set; }

        
        public string RaceId { get; set; }

        [ForeignKey("RaceId"),Column(Order =1)]
        public virtual Race Race { get; set; }



        public int AgeGroupId { get; set; }


        [ForeignKey("AgeGroupId"),Column(Order=2)]
       public virtual AgeGroup AgeGroup { get; set; }


        public int GenderId { get; set; }


        [ForeignKey("GenderId"), Column(Order =3)]
        public virtual Gender Gender { get; set; }



        public DateTime? LastRequestedUTC { get; set; }

        public string Status { get; set; }  //eg: OK , exception string

        public RequestInstruction Instruction { get; set; }  //eg: always get from source, ignore request,  

        public int SourceCount { get; set; } //number of records we got from the source

        public int Expected { get; set; } //number of records we expected to get from source
    }

    public enum RequestInstruction
    {
        Normal=0,
        ForceSource=1, //force a request from the source


    }
}