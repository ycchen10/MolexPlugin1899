﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using Basic;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace MolexPlugin.Model
{
    /// <summary>
    /// 电极间隙
    /// </summary>
    [Serializable]
    public class ElectrodeGapValueInfo : ISetAttribute, ICloneable
    {
        /// <summary>
        /// 粗放
        /// </summary>
        public double CrudeInter { get; set; } = 0;
        /// <summary>
        /// 粗放个数
        /// </summary>
        public int CrudeNum { get; set; } = 0;
        /// <summary>
        /// 中放
        /// </summary>
        public double DuringInter { get; set; } = 0;
        /// <summary>
        /// 中放个数
        /// </summary>
        public int DuringNum { get; set; } = 0;
        /// <summary>
        /// 精放
        /// </summary>
        public double FineInter { get; set; } = 0;
        /// <summary>
        /// 精放个数
        /// </summary>
        public int FineNum { get; set; } = 0;
        /// <summary>
        /// ER 个数
        /// </summary>
        public int[] ERNum
        {
            get;
            set;
        } = new int[2];
        /// <summary>
        /// 设置属性
        /// </summary>
        public bool SetAttribute(Part obj)
        {
            try
            {
                AttributeUtils.AttributeOperation("CrudeInter", this.CrudeInter, obj);
                AttributeUtils.AttributeOperation("CrudeNum", this.CrudeNum, obj);
                AttributeUtils.AttributeOperation("DuringInter", this.DuringInter, obj);
                AttributeUtils.AttributeOperation("DuringNum", this.DuringNum, obj);
                AttributeUtils.AttributeOperation("FineInter", this.FineInter, obj);
                AttributeUtils.AttributeOperation("FineNum", this.FineNum, obj);
                if (this.ERNum[0] != 0 || this.ERNum[1] != 0)
                    AttributeUtils.AttributeOperation("ERNum", this.ERNum, obj);
                return true;
            }
            catch (NXException ex)
            {
                ClassItem.WriteLogFile("写入属性错误！" + ex.Message);
                return false;
            }


        }
        /// <summary>
        /// 读取属性
        /// </summary>
        public static ElectrodeGapValueInfo GetAttribute(NXObject obj)
        {
            ElectrodeGapValueInfo info = new ElectrodeGapValueInfo();
            try
            {
                info.CrudeInter = AttributeUtils.GetAttrForDouble(obj, "CrudeInter");
                info.CrudeNum = AttributeUtils.GetAttrForInt(obj, "CrudeNum");
                info.DuringInter = AttributeUtils.GetAttrForDouble(obj, "DuringInter");
                info.DuringNum = AttributeUtils.GetAttrForInt(obj, "DuringNum");
                info.FineInter = AttributeUtils.GetAttrForDouble(obj, "FineInter");
                info.FineNum = AttributeUtils.GetAttrForInt(obj, "FineNum");
                for (int i = 0; i < 2; i++)
                {
                    info.ERNum[i] = AttributeUtils.GetAttrForInt(obj, "ERNum", i);
                }
                return info;
            }
            catch (NXException ex)
            {
                throw ex;
            }

        }

        public object Clone()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);
            ms.Seek(0, SeekOrigin.Begin);
            return bf.Deserialize(ms);
        }
        /// <summary>
        /// 写入属性
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public bool SetAttribute(params NXObject[] objs)
        {
            try
            {
                AttributeUtils.AttributeOperation("CrudeInter", this.CrudeInter, objs);
                AttributeUtils.AttributeOperation("CrudeNum", this.CrudeNum, objs);
                AttributeUtils.AttributeOperation("DuringInter", this.DuringInter, objs);
                AttributeUtils.AttributeOperation("DuringNum", this.DuringNum, objs);
                AttributeUtils.AttributeOperation("FineInter", this.FineInter, objs);
                AttributeUtils.AttributeOperation("FineNum", this.FineNum, objs);
                if (this.ERNum[0] != 0 || this.ERNum[1] != 0)
                    AttributeUtils.AttributeOperation("ERNum", this.ERNum, objs);
                return true;
            }
            catch (NXException ex)
            {
                ClassItem.WriteLogFile("写入属性错误！" + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 设置ER属性
        /// </summary>
        /// <param name="bp"></param>
        /// <returns></returns>
        public bool SetERToolh(List<ElectrodeToolhInfo[,]> elfs)
        {
            List<ElectrodeToolhInfo> ef = new List<ElectrodeToolhInfo>();
            if (this.ERNum[0] != 0 || this.ERNum[1] != 0)
            {
                foreach (ElectrodeToolhInfo[,] et in elfs)
                {
                    foreach (ElectrodeToolhInfo ei in et)
                    {
                        ef.Add(ei);
                    }
                    for (int i = 0; i < this.ERNum[0]; i++)
                    {
                        for (int k = 0; k < this.ERNum[1]; k++)
                        {
                            foreach (BodyInfo bi in et[i, k].BodyInfos)
                            {
                                bi.ER = true;
                            }
                            ef.Remove(et[i, k]);
                        }
                    }  //粗放个数
                    foreach (ElectrodeToolhInfo ei in ef)
                    {
                        foreach (BodyInfo bi in ei.BodyInfos)
                        {
                            bi.EF = true;
                        }
                    }
                    ef.Clear();               
                }
                return true;
            }
            else
            {
                foreach (ElectrodeToolhInfo[,] et in elfs)
                {
                    foreach (ElectrodeToolhInfo ei in et)
                    {
                        foreach (BodyInfo bi in ei.BodyInfos)
                        {
                            bi.EF = false;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="table"></param>   
        public static void CreateDataTable(ref DataTable table)
        {
            foreach (PropertyInfo propertyInfo in typeof(ElectrodeGapValueInfo).GetProperties())  //以属性添加列
            {
                try
                {
                    table.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            try
            {
                table.Columns.Add("ER-X", Type.GetType("System.Int32"));
                table.Columns.Add("ER-Y", Type.GetType("System.Int32"));
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        /// <summary>
        ///创建行
        /// </summary>
        /// <param name="row"></param>
        public void CreateDataRow(ref DataRow row)
        {
            ElectrodeGapValueInfo info = this.Clone() as ElectrodeGapValueInfo;
            foreach (PropertyInfo propertyInfo in typeof(ElectrodeGapValueInfo).GetProperties())
            {
                try
                {
                    row[propertyInfo.Name] = propertyInfo.GetValue(info, null);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            try
            {
                row["ER-X"] = info.ERNum[0];
                row["ER-Y"] = info.ERNum[1];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 通过行获取数据
        /// </summary>
        /// <param name="row"></param>
        public static ElectrodeGapValueInfo GetInfoForDataRow(DataRow row)
        {
            ElectrodeGapValueInfo info = new ElectrodeGapValueInfo();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                try
                {
                    PropertyInfo propertyInfo = info.GetType().GetProperty(row.Table.Columns[i].ColumnName);
                    if (propertyInfo != null && row[i] != DBNull.Value)
                        propertyInfo.SetValue(info, row[i], null);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            try
            {
                info.ERNum[0] = Convert.ToInt32(row["ER-X"]);
                info.ERNum[0] = Convert.ToInt32(row["ER-X"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return info;
        }
        /// <summary>
        /// 比较是否修改电极
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsEquals(ElectrodeGapValueInfo other)
        {
            return this.ERNum[0] == other.ERNum[0] &&
                 this.ERNum[1] == other.ERNum[1];

        }
    }
}
