//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Thalili.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sub_order
    {
        public int user_id { get; set; }
        public int medical_analysis_id { get; set; }
        public int lab_id { get; set; }
        public Nullable<int> count { get; set; }
        public Nullable<bool> is_finshed { get; set; }
        public string pdf { get; set; }
        public int order_id { get; set; }
    
        public virtual analysis_in_lab analysis_in_lab { get; set; }
        public virtual order order { get; set; }
        public virtual user user { get; set; }
    }
}
