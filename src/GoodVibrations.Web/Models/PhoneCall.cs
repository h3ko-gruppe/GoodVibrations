using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodVibrations.Web.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GoodVibrations.Web.Models
{
    public class PhoneCall : BaseModel
    {
       public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ToPhoneNumber { get; set; }

        public string FromPhoneNumber { get; set; }

        public string Message { get; set; }

        public string CurrentLocation { get; set; }

        public string Token { get; set; }

        internal class Mapping : BaseMapping<PhoneCall>
        {
            public override void Map(EntityTypeBuilder<PhoneCall> b)
            {
                b.HasKey(x => x.Id);
                b.ToTable(Const.TableNames.PhoneCall);
                b.Property(x => x.CreatedAt).IsRequired();
                b.Property(x => x.ToPhoneNumber).IsRequired();
                b.Property(x => x.FromPhoneNumber).IsRequired(false);
                b.Property(x => x.Message).IsRequired();
                b.Property(x => x.CurrentLocation).IsRequired(false);
                b.Property(x => x.Token).IsRequired();

                b.HasOne(x => x.User)
                    .WithMany(x => x.PhoneCalls)
                    .HasForeignKey(x => x.UserId);
            }
        }

    }
}
