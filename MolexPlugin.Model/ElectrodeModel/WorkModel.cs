﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.UF;
using NXOpen.BlockStyler;
using System.IO;
using NXOpen.Assemblies;
using Basic;

namespace MolexPlugin.Model
{
    /// <summary>
    /// Work装配档
    /// </summary>
    public class WorkModel : AbstractAssmbileModel, IComparable<WorkModel>
    {
        public WorkInfo Info { get; protected set; }
        public WorkModel(Part part) : base(part)
        {

        }

        public WorkModel(WorkInfo info)
        {
            this.Info = info;
        }

        public int CompareTo(WorkModel other)
        {
            return this.Info.WorkNumber.CompareTo(other.Info.WorkNumber);
        }

        public override string GetAssembleName()
        {
            return this.Info.MoldInfo.MoldNumber + "-" + this.Info.MoldInfo.WorkpieceNumber + "-" + "WORK" + this.Info.WorkNumber.ToString();
        }

        public override Component Load(Part parentPart)
        {
            Matrix4 matr = new Matrix4();
            matr.Identity();
            try
            {
                Component ct = Basic.AssmbliesUtils.PartLoad(parentPart, this.WorkpiecePath, this.AssembleName, matr, new Point3d(0, 0, 0));
                return ct;

            }
            catch (NXException ex)
            {
                throw ex;
            }
        }

        protected override void GetModelForAttribute(NXObject obj)
        {
            this.Info = WorkInfo.GetAttribute(obj);
        }
        // 修改矩阵
        /// </summary>
        /// <param name="matr"></param>
        public bool AlterMatr(Matrix4 matr)
        {
            this.Info.Matr = matr;
            this.Info.MatrInfo = new Matrix4Info(matr);
            return this.Info.SetAttribute(this.PartTag);
        }
        /// <summary>
        /// 判断当前部件是否是Work
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public static bool IsWork(Part part)
        {
            ParentAssmblieInfo info = ParentAssmblieInfo.GetAttribute(part);
            return info.Type == PartType.Work;
        }

        /// <summary>
        /// 保存坐标
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public bool SaveCsys(Part asm)
        {
            UFSession theUFSession = UFSession.GetUFSession();
            try
            {
                PartUtils.SetPartDisplay(asm);
                List<NXOpen.Assemblies.Component> ct = AssmbliesUtils.GetPartComp(asm, this.PartTag);
                PartUtils.SetPartWork(ct[0]);
                CartesianCoordinateSystem csys = asm.WCS.Save();
                string name = "WORK" + this.Info.WorkNumber.ToString();
                Tag objTag = Tag.Null;
                theUFSession.Obj.CycleByName(name, ref objTag);
                if (objTag != Tag.Null)
                {
                    theUFSession.Obj.DeleteObject(objTag);
                }
                csys.Name = name;
                csys.Layer = 200;
                csys.Color = 186;
                PartUtils.SetPartWork(null);
                return true;
            }
            catch (NXException ex)
            {
                ClassItem.WriteLogFile("创建坐标错误" + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 设置是否做了过切检查
        /// </summary>
        /// <param name="interference"></param>
        public void SetInterference(bool interference)
        {
            bool fe = this.Info.Interference;
            if (fe != interference)
            {
                try
                {
                    AttributeUtils.AttributeOperation("Interference", interference, this.PartTag);
                    this.Info.Interference = interference;
                }
                catch (NXException ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 获取主工件
        /// </summary>
        /// <returns></returns>
        public Part GetHostWorkpiece()
        {
            string name = this.Info.MoldInfo.MoldNumber + this.Info.MoldInfo.WorkpieceNumber + this.Info.MoldInfo.EditionNumber;
            try
            {
                foreach (Part pt in Session.GetSession().Parts)
                {
                    if (name.Replace("-", "").Equals(pt.Name.Replace("-", ""), StringComparison.CurrentCultureIgnoreCase))
                    {
                        List<NXOpen.Assemblies.Component> ct = AssmbliesUtils.GetPartComp(this.PartTag, pt);
                        if (ct.Count > 0)
                            return pt;
                    }
                }
            }
            catch
            {

            }
            return null;
        }
        public override bool SetAttribute(NXObject obj)
        {
            return this.Info.SetAttribute(obj);
        }
    }
}