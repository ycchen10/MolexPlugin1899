﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using Basic;

namespace MolexPlugin.DAL
{
    /// <summary>
    /// 以轴绕度数
    /// </summary>
    public class MoveRotateBuilder : IMoveBulider
    {
        private Vector3d vec;
        private double angle;
        private UserSingleton user;
        public MoveRotateBuilder(Vector3d vec, double angle)
        {
            this.vec = vec;
            this.angle = angle;
            this.user = UserSingleton.Instance();
        }
        public NXObject Move(params NXObject[] objs)
        {
            if (user.UserSucceed&&user.Jurisd.GetComm())
            {
                try
                {
                    NXObject obj = MoveObject.MoveObjectOfRotate(this.vec, this.angle, objs);
                    return obj;
                }
                catch(NXException ex)
                {
                    UI.GetUI().NXMessageBox.Show("错误", NXMessageBox.DialogType.Error, "无法移动--" + ex.Message);
                    return null;
                }
               
            }
            else
            {

                return null;
            }

        }
    }
}
