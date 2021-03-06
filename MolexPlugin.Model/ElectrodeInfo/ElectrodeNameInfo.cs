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
using System.Text.RegularExpressions;

namespace MolexPlugin.Model
{
    /// <summary>
    /// 电极CAM信息
    /// </summary>
    [Serializable]
    public class ElectrodeNameInfo : ISetAttribute, ICloneable
    {
        /// <summary>
        /// 电极名
        /// </summary>
        public string EleName { get; set; } = "";
        /// <summary>
        /// 借用电极
        /// </summary>
        public string BorrowName { get; set; } = "";
        /// <summary>
        /// 电极编号
        /// </summary>
        public int EleNumber { get; set; }

        public string EleEditionNumber { get; set; } = "A";

        /// <summary>
        /// 设置属性
        /// </summary>
        public bool SetAttribute(Part obj)
        {

            try
            {
                AttributeUtils.AttributeOperation("EleName", this.EleName, obj);
                AttributeUtils.AttributeOperation("BorrowName", this.BorrowName, obj);
                AttributeUtils.AttributeOperation("EleNumber", this.EleNumber, obj);
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
        public static ElectrodeNameInfo GetAttribute(NXObject obj)
        {
            ElectrodeNameInfo info = new ElectrodeNameInfo();
            try
            {
                info.EleName = AttributeUtils.GetAttrForString(obj, "EleName");
                info.BorrowName = AttributeUtils.GetAttrForString(obj, "BorrowName");
                info.EleNumber = AttributeUtils.GetAttrForInt(obj, "EleNumber");
                info.EleEditionNumber = AttributeUtils.GetAttrForString(obj, "EleEditionNumber");
                return info;
            }
            catch (NXException ex)
            {
                throw ex;
            }

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
                AttributeUtils.AttributeOperation("EleName", this.EleName, objs);
                AttributeUtils.AttributeOperation("BorrowName", this.BorrowName, objs);
                AttributeUtils.AttributeOperation("EleNumber", this.EleNumber, objs);
                AttributeUtils.AttributeOperation("EleEditionNumber", this.EleEditionNumber, objs);
                return true;
            }
            catch (NXException ex)
            {
                ClassItem.WriteLogFile("写入属性错误！" + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);
            ms.Seek(0, SeekOrigin.Begin);
            return bf.Deserialize(ms);
        }
        /// <summary>
        /// 获取电极号
        /// </summary>
        /// <param name="eleName"></param>
        /// <returns></returns>
        public int GetEleNumber(string eleName)
        {
            string name = eleName.Substring(eleName.LastIndexOf("E"));
            MatchCollection match = Regex.Matches(name, @"\d+");
            int result;
            if (match.Count != 0 && int.TryParse(match[0].Value, out result))
            {
                return result;
            }
            return 0;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="table"></param>   
        public static void CreateDataTable(ref DataTable table)
        {
            foreach (PropertyInfo propertyInfo in typeof(ElectrodeNameInfo).GetProperties())  //以属性添加列
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

        }
        /// <summary>
        ///创建行
        /// </summary>
        /// <param name="row"></param>
        public void CreateDataRow(ref DataRow row)
        {
            ElectrodeNameInfo info = this.Clone() as ElectrodeNameInfo;
            foreach (PropertyInfo propertyInfo in typeof(ElectrodeNameInfo).GetProperties())
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
        }
        /// <summary>
        /// 通过行获取数据
        /// </summary>
        /// <param name="row"></param>
        public static ElectrodeNameInfo GetInfoForDataRow(DataRow row)
        {
            ElectrodeNameInfo info = new ElectrodeNameInfo();
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
            return info;
        }
    }
}

