using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using GCP.Enum;

namespace GCP.Entity
{
    public class BaseNoPrimaryEntity : BaseNotPrimaryIDEntity
    {
        public BaseNoPrimaryEntity()
        {
            this.SetStatus(Enum.Status.正常);
        }

        /// <summary>
        /// 状态，1正常，2删除
        /// </summary>
        [JsonProperty]
        public long Status { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty]
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

    public class BaseNotPrimaryIDEntity
    {
        /// <summary>
        /// ID主键
        /// </summary>
        [JsonProperty, Column(IsIdentity = true)]
        public long ID { get; set; }
    }
}
