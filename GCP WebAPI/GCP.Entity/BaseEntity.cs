using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using GCP.Enum;
using GCP.Util;

namespace GCP.Entity
{
    public class BaseEntity : BaseIDEntity
    {
        public BaseEntity()
        {
            this.SetStatus(Enum.Status.正常);
        }

        /// <summary>
        /// 状态，1正常，2删除
        /// </summary>
        [JsonProperty, Column(Name = "status")]
        public long Status { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty, Column(Name = "updatetime")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime UpdateTime { get; set; }

        public virtual void Create()
        {
            this.Update();
        }

        public virtual void Update()
        {
            this.UpdateTime = DateTime.Now;
        }

        public virtual void Modify()
        {
            this.Update();
        }

        public virtual void NotVerify()
        {
            this.SetStatus(Enum.Status.未审核);
        }

        public virtual void Verify()
        {
            this.SetStatus(Enum.Status.正常);
        }

        public virtual void Enable()
        {
            this.SetStatus(Enum.Status.正常);
        }

        public virtual void Delete()
        {
            this.SetStatus(Enum.Status.删除);
        }

        public virtual void Disable()
        {
            this.SetStatus(Enum.Status.禁用);
        }

        public virtual void SetStatus(Status status)
        {
            this.Update();
            this.Status = status.ToInt64();
        }
    }

    public class BaseIDEntity
    {
        /// <summary>
        /// ID主键
        /// </summary>
        [JsonProperty, Column(IsPrimary = true, IsIdentity = true,Name ="id")]
        public long ID { get; set; }
    }

    public class BaseField
    {
        public static string[] BaseFieldList = new string[]
        {
            "ID".ToUpper(),
            "STATUS".ToUpper(),
            "UPDATETIME".ToUpper(),
            "Level1_ID".ToUpper(),
            "Level1_Uploaded".ToUpper(),
            "Level1_UploadTime".ToUpper(),
            "Level1_UploadCount".ToUpper(),
            "UploadTime".ToUpper()
        };
    }
}
