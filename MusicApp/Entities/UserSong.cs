//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserSong
    {
        public int UserID { get; set; }
        public int SongID { get; set; }
        public Nullable<bool> Listened { get; set; }
        public Nullable<byte> Rating { get; set; }
        public Nullable<int> ITunesID { get; set; }
    
        public virtual Song Song { get; set; }
        public virtual User User { get; set; }
    }
}
