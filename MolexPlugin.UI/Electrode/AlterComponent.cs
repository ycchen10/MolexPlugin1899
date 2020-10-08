﻿//==============================================================================
//  WARNING!!  This file is overwritten by the Block UI Styler while generating
//  the automation code. Any modifications to this file will be lost after
//  generating the code again.
//
//       Filename:  C:\Users\ycchen10\OneDrive - kochind.com\Desktop\MolexPlugIn-1899\UI\AlterComponent.cs
//
//        This file was generated by the NX Block UI Styler
//        Created by: ycchen10
//              Version: NX 11
//              Date: 09-23-2020  (Format: mm-dd-yyyy)
//              Time: 14:12 (Format: hh-mm)
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;
using NXOpen.BlockStyler;
using NXOpen.Utilities;
using MolexPlugin.DAL;
using NXOpen.UF;
using Basic;
using MolexPlugin.Model;
using System.IO;

namespace MolexPlugin
{
    //------------------------------------------------------------------------------
    //Represents Block Styler application class
    //------------------------------------------------------------------------------
    public partial class AlterComponent
    {
        //class members
        private static Session theSession = null;
        private static NXOpen.UI theUI = null;
        private string theDlxFileName;
        private NXOpen.BlockStyler.BlockDialog theDialog;
        private NXOpen.BlockStyler.Group group0;// Block type: Group
        private NXOpen.BlockStyler.SelectObject seleComp;// Block type: Selection
        private NXOpen.BlockStyler.Group groupWorkpiece;// Block type: Group
        private NXOpen.BlockStyler.StringBlock strMoldNumber;// Block type: String
        private NXOpen.BlockStyler.StringBlock strWorkpieceNumber;// Block type: String
        private NXOpen.BlockStyler.StringBlock strEditionNumber;// Block type: String
        private NXOpen.BlockStyler.Group groupEle;// Block type: Group
        private NXOpen.BlockStyler.StringBlock strEleName;// Block type: String
        private NXOpen.BlockStyler.StringBlock strEleName1;// Block type: String
        private NXOpen.BlockStyler.StringBlock strEleEditionNumber;// Block type: String
        private ParentAssmblieInfo info;
        private UserSingleton user;
        //------------------------------------------------------------------------------
        //Constructor for NX Styler class
        //------------------------------------------------------------------------------
        public AlterComponent()
        {
            try
            {
                theSession = Session.GetSession();
                theUI = NXOpen.UI.GetUI();
                theDlxFileName = "AlterComponent.dlx";
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
                user = UserSingleton.Instance();
                if (user.UserSucceed && user.Jurisd.GetElectrodeJurisd())
                {
                    Part disPart = theSession.Parts.Display;
                    if (!ASMModel.IsAsm(disPart))
                    {
                        ASMModel asm = ASMCollection.GetAsmModel(disPart);
                        if (asm != null)
                            PartUtils.SetPartDisplay(asm.PartTag);
                        else
                        {
                            theUI.NXMessageBox.Show("错误", NXMessageBox.DialogType.Error, "工作部件无法找到ASM!");
                            return 0;
                        }
                    }
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
                seleComp = (NXOpen.BlockStyler.SelectObject)theDialog.TopBlock.FindBlock("seleComp");
                groupWorkpiece = (NXOpen.BlockStyler.Group)theDialog.TopBlock.FindBlock("groupWorkpiece");
                strMoldNumber = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("strMoldNumber");
                strWorkpieceNumber = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("strWorkpieceNumber");
                strEditionNumber = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("strEditionNumber");
                groupEle = (NXOpen.BlockStyler.Group)theDialog.TopBlock.FindBlock("groupEle");
                strEleName = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("strEleName");
                strEleName1 = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("strEleName1");
                strEleEditionNumber = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("strEleEditionNumber");
                Selection.MaskTriple maskComp = new Selection.MaskTriple()
                {
                    Type = 63,
                    Subtype = 1,
                    SolidBodySubtype = 0
                };
                Selection.MaskTriple[] masks = { maskComp };
                seleComp.SetSelectionFilter(Selection.SelectionAction.ClearAndEnableSpecific, masks);//过滤只选择组件
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
                this.groupEle.Show = false;
                this.groupWorkpiece.Show = false;

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
                NXOpen.Assemblies.Component ct = seleComp.GetSelectedObjects()[0] as NXOpen.Assemblies.Component;
                if (ParentAssmblieInfo.IsElectrode(ct))
                {
                    AlterEle(ct);
                }
                else
                {
                    AlterWorkpiece(ct, user.CreatorUser);
                }
                bool anyPartsModified;
                PartSaveStatus saveStatus;
                theSession.Parts.SaveAll(out anyPartsModified, out saveStatus);
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
                if (block == seleComp)
                {
                    //---------Enter your code here-----------
                    TaggedObject[] tt = seleComp.GetSelectedObjects();
                    if (tt.Length > 0)
                    {
                        NXOpen.Assemblies.Component ct = tt[0] as NXOpen.Assemblies.Component;
                        if (ct != null)
                            SetDisp(ct);                     
                    }
                }
                else if (block == strMoldNumber)
                {
                    //---------Enter your code here-----------
                }
                else if (block == strWorkpieceNumber)
                {
                    //---------Enter your code here-----------
                }
                else if (block == strEditionNumber)
                {
                    //---------Enter your code here-----------
                }
                else if (block == strEleName)
                {
                    //---------Enter your code here-----------
                }
                else if (block == strEleName1)
                {
                    //---------Enter your code here-----------
                }
                else if (block == strEleEditionNumber)
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

        //------------------------------------------------------------------------------
        //Callback Name: filter_cb
        //------------------------------------------------------------------------------
        public int filter_cb(NXOpen.BlockStyler.UIBlock block, NXOpen.TaggedObject selectedObject)
        {
            NXOpen.Assemblies.Component ct = selectedObject as NXOpen.Assemblies.Component;
            if (ct != null)
            {
                if (ParentAssmblieInfo.IsElectrode(ct) || ParentAssmblieInfo.IsWorkpiece(ct) || !ParentAssmblieInfo.IsParent(ct))
                    return (NXOpen.UF.UFConstants.UF_UI_SEL_ACCEPT);
            }
            return (NXOpen.UF.UFConstants.UF_UI_SEL_FAILURE);
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

    }
}
