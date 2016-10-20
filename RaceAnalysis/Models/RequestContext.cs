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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestContextId { get; set; }

        [ForeignKey("Race")]
        public int RaceId { get; set; }
        public virtual Race Race { get; set; }


        [ForeignKey("Gender")]
        public int GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        [ForeignKey("AgeGroup")]
        public int AgeGroupId { get; set; }
        public virtual AgeGroup AgeGroup { get; set; }

        public DateTime? LastRequestedUTC { get; set; }

        public string Status { get; set; }  //eg: OK , exception string

        public RequestInstruction Instruction { get; set; }  //eg: always get from source, ignore request,  

        public int SourceCount { get; set; } //number of records we got from the source
    }

    public enum RequestInstruction
    {
        Normal=0,
        ForceSource=1, //force a request from the source


    }
}