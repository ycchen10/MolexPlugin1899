﻿//==============================================================================
//  WARNING!!  This file is overwritten by the Block UI Styler while generating
//  the automation code. Any modifications to this file will be lost after
//  generating the code again.
//
//       Filename:  C:\Users\ycchen10\OneDrive - kochind.com\Desktop\MolexPlugIn-12.0\UI\AddEdmAsm.cs
//
//        This file was generated by the NX Block UI Styler
//        Created by: ycchen10
//              Version: NX 11
//              Date: 02-26-2020  (Format: mm-dd-yyyy)
//              Time: 10:59 (Format: hh-mm)
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
    public partial class AddEdmAsm
    {
        //class members
        private static Session theSession = null;
        private static NXOpen.UI theUI = null;
        private string theDlxFileName;
        private NXOpen.BlockStyler.BlockDialog theDialog;
        private NXOpen.BlockStyler.Group group0;// Block type: Group
        private NXOpen.BlockStyler.StringBlock MoldNumber;// Block type: String
        private NXOpen.BlockStyler.StringBlock PartNumber;// Block type: String
        private NXOpen.BlockStyler.StringBlock EditionNumber;// Block type: String
        private NXOpen.BlockStyler.StringBlock MoldType;// Block type: String
        private NXOpen.BlockStyler.StringBlock ClientNumber;// Block type: String
        private NXOpen.BlockStyler.Group group;// Block type: Group
        private NXOpen.BlockStyler.StringBlock MachineType;// Block type: String
        private Part workPart;
        //------------------------------------------------------------------------------
        //Constructor for NX Styler class
        //------------------------------------------------------------------------------
        public AddEdmAsm()
        {
            try
            {
                theSession = Session.GetSession();
                theUI = NXOpen.UI.GetUI();
                workPart = theSession.Parts.Work;
                theDlxFileName = "AddEdmAsm.dlx";
                theDialog = theUI.CreateDialog(theDlxFileName);
                theDialog.AddApplyHandler(new NXOpen.BlockStyler.BlockDialog.Apply(apply_cb));
                theDialog.AddOkHandler(new NXOpen.BlockStyler.BlockDialog.Ok(ok_cb));
                theDialog.AddUpdateHandler(new NXOpen.BlockStyler.BlockDialog.Update(update_cb));
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
                UserSingleton user = UserSingleton.Instance();
                if (user.UserSucceed && user.Jurisd.GetElectrodeJurisd())
                {
                    if (workPart.ComponentAssembly.RootComponent != null)
                    {
                        theUI.NXMessageBox.Show("错误", NXMessageBox.DialogType.Error, "工件是装配档");
                        return 0;
                    }
                    if (workPart.PartUnits == BasePart.Units.Inches)
                    {
                        theUI.NXMessageBox.Show("错误", NXMessageBox.DialogType.Error, "工件是英制");
                        return 0;
                    }
                    workPart.Save(BasePart.SaveComponents.False, BasePart.CloseAfterSave.False);
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
                MoldNumber = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("MoldNumber");
                PartNumber = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("PartNumber");
                EditionNumber = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("EditionNumber");
                MoldType = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("MoldType");
                ClientNumber = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("ClientNumber");
                group = (NXOpen.BlockStyler.Group)theDialog.TopBlock.FindBlock("group");
                MachineType = (NXOpen.BlockStyler.StringBlock)theDialog.TopBlock.FindBlock("MachineType");
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
                this.SetUiInfo();
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
                MoldInfo info = new MoldInfo()
                {
                    MoldNumber = this.MoldNumber.WideValue.ToUpper(),
                    WorkpieceNumber = this.PartNumber.WideValue.ToUpper(),
                    EditionNumber = this.EditionNumber.WideValue.ToUpper(),
                    ClientName = this.ClientNumber.WideValue,
                    MoldType = this.MoldType.WideValue.ToUpper(),
                    MachineType = this.MachineType.WideValue.ToUpper()
                };
                string directoryPath = GetDirectoryPath(info);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath); //创建件号文件夹
                }
                CreateBulder(info, directoryPath);
                bool anyPartsModified1;
                NXOpen.PartSaveStatus partSaveStatus1;
                Session.GetSession().Parts.SaveAll(out anyPartsModified1, out partSaveStatus1);
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
                if (block == MoldNumber)
                {
                    //---------Enter your code here-----------
                }
                else if (block == PartNumber)
                {
                    //---------Enter your code here-----------
                }
                else if (block == EditionNumber)
                {
                    //---------Enter your code here-----------
                }
                else if (block == MoldType)
                {
                    //---------Enter your code here-----------
                }
                else if (block == ClientNumber)
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
