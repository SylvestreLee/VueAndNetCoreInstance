using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    public class InsVehicle 
    {
        /// <summary>
        /// 检测记录实体
        /// </summary>
        public InspectionEntity inspectionEntity { get; set; }

        /// <summary>
        /// 车辆信息实体
        /// </summary>
        public VehicleEntity vehicleEntity { get; set; }
    }

}
