﻿//==============================================================================
//  WARNING!!  This file is overwritten by the Block UI Styler while generating
//  the automation code. Any modifications to this file will be lost after
//  generating the code again.
//
//       Filename:  C:\Users\ycchen10\OneDrive - kochind.com\Desktop\MolexPlugIn-1899\UI\ElectrodeColor.cs
//
//        This file was generated by the NX Block UI Styler
//        Created by: ycchen10
//              Version: NX 1919
//              Date: 10-27-2020  (Format: mm-dd-yyyy)
//              Time: 16:58 (Format: hh-mm)
//
//==============================================================================

//==============================================================================
//  Purpose:  This TEMPLATE file contains C# source to guide you in the
//  construction of your Block application dialog. The generation of your
//  dialog file (.dlx extension) is the first step towards dialog construction
//  within NX.  You must now create a NX Open application that
//  utilizes this file (.dlx).
//
//  The information in this file provides you with the following:
//
//  1.  Help on how to load and display your Block UI Styler dialog in NX
//      using APIs provided in NXOpen.BlockStyler namespace
//  2.  The empty callback methods (stubs) associated with your dialog items
//      have also been placed in this file. These empty methods have been
//      created simply to start you along with your coding requirements.
//      The method name, argument list and possible return values have already
//      been provided for you.
//==============================================================================

//------------------------------------------------------------------------------
//These imports are needed for the following template code
//------------------------------------------------------------------------------
using System;
using NXOpen;
using NXOpen.BlockStyler;
using MolexPlugin.DAL;
using MolexPlugin.Model;
using Basic;
using System.Collections.Generic;

namespace MolexPlugin
{


    //------------------------------------------------------------------------------
    //Represents Block Styler application class
    //------------------------------------------------------------------------------
    public class ElectrodeColor
    {
        //class members
        private static Session theSession = null;
        private static NXOpen.UI theUI = null;
        private string theDlxFileName;
        private NXOpen.BlockStyler.BlockDialog theDialog;
        private NXOpen.BlockStyler.Group group0;// Block type: Group
        private NXOpen.BlockStyler.SelectObject seleEle;// Block type: Selection
        private UserSingleton user = UserSingleton.Instance();

        public ElectrodeColor()
        {
            try
            {
                theSession = Session.GetSession();
                theUI = NXOpen.UI.GetUI();
                theDlxFileName = "ElectrodeColor.dlx";
                theDialog = theUI.CreateDialog(theDlxFileName);
                theDialog.AddApplyHandler(new NXOpen.BlockStyler.BlockDialog.Apply(apply_cb));
                theDialog.AddOkHandler(new NXOpen.BlockStyler.BlockDialog.Ok(ok_cb));
                theDialog.AddUpdateHandler(new NXOpen.BlockStyler.BlockDialog.Update(update_cb));
                theDialog.AddFilterHandler(new NXOpen.BlockStyler.BlockDialog.Filter(filter_cb));
                theDialog.AddInitializeHandler(new NXOpen.BlockStyler.BlockDialog.Initialize(initialize_cb));
                theDialog.AddDialogShownHandler(new NXOpen.BlockStyler.BlockDialog.DialogShown(dialogShown_cb));
            }
            catch (Exception ex)
            {
                //---- Enter your exception handling code here -----
                throw ex;
            }
        }



        //------------------------------------------------------------------------------
        //This method shows the dialog on the screen
        //------------------------------------------------------------------------------
        public NXOpen.UIStyler.DialogResponse Show()
        {
            try
            {
                if (user.UserSucceed && user.Jurisd.GetElectrodeJurisd())
                {
                    theDialog.Show();
                }

            }
            catch (Exception ex)
            {
                //---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
            }
            return 0;
        }

        //------------------------------------------------------------------------------
        //Method Name: Dispose
        //------------------------------------------------------------------------------
        public void Dispose()
        {
            if (theDialog != null)
            {
                theDialog.Dispose();
                theDialog = null;
            }
        }

        //------------------------------------------------------------------------------
        //---------------------Block UI Styler Callback Functions--------------------------
        //------------------------------------------------------------------------------

        //------------------------------------------------------------------------------
        //Callback Name: initialize_cb
        //------------------------------------------------------------------------------
        public void initialize_cb()
        {
            try
            {
                group0 = (NXOpen.BlockStyler.Group)theDialog.TopBlock.FindBlock("group0");
                seleEle = (NXOpen.BlockStyler.SelectObject)theDialog.TopBlock.FindBlock("seleEle");
                Selection.MaskTriple maskComp = new Selection.MaskTriple()
                {
                    Type = 63,
                    Subtype = 1,
                    SolidBodySubtype = 0
                };
                Selection.MaskTriple[] masks = { maskComp };
                seleEle.SetSelectionFilter(Selection.SelectionAction.ClearAndEnableSpecific, masks);//过滤只选择组件
            }
            catch (Exception ex)
            {
                //---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
            }
        }

        //------------------------------------------------------------------------------
        //Callback Name: dialogShown_cb
        //This callback is executed just before the dialog launch. Thus any value set 
        //here will take precedence and dialog will be launched showing that value. 
        //------------------------------------------------------------------------------
        public void dialogShown_cb()
        {
            try
            {
                //---- Enter your callback code here -----
            }
            catch (Exception ex)
            {
                //---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
            }
        }

        //------------------------------------------------------------------------------
        //Callback Name: apply_cb
        //------------------------------------------------------------------------------
        public int apply_cb()
        {
            int errorCode = 0;
            try
            {
                //---- Enter your callback code here -----
                List<Part> elePart = new List<Part>();
                foreach (TaggedObject to in this.seleEle.GetSelectedObjects())
                {
                    if (to is NXOpen.Assemblies.Component)
                    {
                        Part pt = (to as NXOpen.Assemblies.Component).Prototype as Part;
                        if (!elePart.Exists(a => a.Equals(pt)))
                        {
                            elePart.Add(pt);
                        }
                    }
                }

                foreach (Part pt in elePart)
                {
                    foreach (Body by in pt.Bodies)
                    {
                        SetEleColor(by);
                    }
                }
            }
            catch (Exception ex)
            {
                //---- Enter your exception handling code here -----
                errorCode = 1;
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
            }
            return errorCode;
        }

        //------------------------------------------------------------------------------
        //Callback Name: update_cb
        //------------------------------------------------------------------------------
        public int update_cb(NXOpen.BlockStyler.UIBlock block)
        {
            try
            {
                if (block == seleEle)
                {
                    //---------Enter your code here-----------
                }
            }
            catch (Exception ex)
            {
                //---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
            }
            return 0;
        }

        //------------------------------------------------------------------------------
        //Callback Name: ok_cb
        //------------------------------------------------------------------------------
        public int ok_cb()
        {
            int errorCode = 0;
            try
            {
                errorCode = apply_cb();
                //---- Enter your callback code here -----
            }
            catch (Exception ex)
            {
                //---- Enter your exception handling code here -----
                errorCode = 1;
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
            }
            return errorCode;
        }
        public int filter_cb(NXOpen.BlockStyler.UIBlock block, NXOpen.TaggedObject selectedObject)
        {
            if (selectedObject is NXOpen.Assemblies.Component)
            {
                Part part = (selectedObject as NXOpen.Assemblies.Component).Prototype as Part;
                ParentAssmblieInfo info = ParentAssmblieInfo.GetAttribute(part);
                if (info.Type == PartType.Electrode)
                    return (NXOpen.UF.UFConstants.UF_UI_SEL_ACCEPT);
            }

            return (NXOpen.UF.UFConstants.UF_UI_SEL_REJECT);
        }
        //------------------------------------------------------------------------------
        //Function Name: GetBlockProperties
        //Returns the propertylist of the specified BlockID
        //------------------------------------------------------------------------------
        public PropertyList GetBlockProperties(string blockID)
        {
            PropertyList plist = null;
            try
            {
                plist = theDialog.GetBlockProperties(blockID);
            }
            catch (Exception ex)
            {
                //---- Enter your exception handling code here -----
                theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
            }
            return plist;
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="by"></param>
        private void SetEleColor(Body by)
        {
            foreach (Face fe in by.GetFaces())
            {
                string gap = AttributeUtils.GetAttrForString(fe, "ToolhGapValue");
                if (gap.Equals("ER", StringComparison.CurrentCultureIgnoreCase))
                    fe.Color = 6;
                if (gap.Equals("EF", StringComparison.CurrentCultureIgnoreCase))
                    fe.Color = 108;
            }
        }

    }
}
