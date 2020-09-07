﻿//==============================================================================
//  WARNING!!  This file is overwritten by the Block UI Styler while generating
//  the automation code. Any modifications to this file will be lost after
//  generating the code again.
//
//       Filename:  C:\Users\ycchen10\OneDrive - kochind.com\Desktop\MolexPlugIn-1899\UI\CheckElectrode.cs
//
//        This file was generated by the NX Block UI Styler
//        Created by: ycchen10
//              Version: NX 11
//              Date: 08-31-2020  (Format: mm-dd-yyyy)
//              Time: 15:50 (Format: hh-mm)
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
using NXOpen.UF;
using NXOpen.BlockStyler;
using MolexPlugin.DAL;
using Basic;

namespace MolexPlugin
{
    //------------------------------------------------------------------------------
    //Represents Block Styler application class
    //------------------------------------------------------------------------------
    public class CheckElectrode
    {
        //class members
        private static Session theSession = null;
        private static UI theUI = null;
        private string theDlxFileName;
        private NXOpen.BlockStyler.BlockDialog theDialog;
        private NXOpen.BlockStyler.Group Group;// Block type: Group
        private NXOpen.BlockStyler.SelectObject Selection;// Block type: Selection
        private NXOpen.BlockStyler.Group Group1;// Block type: Group
        private NXOpen.BlockStyler.SelectObject Selection1;// Block type: Selection

        //------------------------------------------------------------------------------
        //Constructor for NX Styler class
        //------------------------------------------------------------------------------
        public CheckElectrode()
        {
            try
            {
                theSession = Session.GetSession();
                theUI = UI.GetUI();
                theDlxFileName = "CheckElectrode.dlx";
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
                theDialog.Show();
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
                Group = (NXOpen.BlockStyler.Group)theDialog.TopBlock.FindBlock("Group");
                Selection = (NXOpen.BlockStyler.SelectObject)theDialog.TopBlock.FindBlock("Selection");
                Group1 = (NXOpen.BlockStyler.Group)theDialog.TopBlock.FindBlock("Group1");
                Selection1 = (NXOpen.BlockStyler.SelectObject)theDialog.TopBlock.FindBlock("Selection1");
                Selection.MaskTriple maskBody = new Selection.MaskTriple() //面
                {
                    Type = 70,
                    Subtype = 0,
                    SolidBodySubtype = UFConstants.UF_UI_SEL_FEATURE_BODY
                };
                Selection.MaskTriple[] masks = { maskBody };
                this.Selection.SetSelectionFilter(NXOpen.Selection.SelectionAction.ClearAndEnableSpecific, masks);
                this.Selection1.SetSelectionFilter(NXOpen.Selection.SelectionAction.ClearAndEnableSpecific, masks);
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
                List<Body> toolBodys = new List<Body>();
                List<Body> eleBodys = new List<Body>();
                Matrix4 mat = new Matrix4();
                mat.Identity();
                Matrix4 inv = mat.GetInversMatrix();
                CartesianCoordinateSystem csys = BoundingBoxUtils.CreateCoordinateSystem(mat, inv);
                foreach (TaggedObject tt in this.Selection.GetSelectedObjects())
                {
                    Body ttBody = tt as Body;
                    if (ttBody.Prototype != null)
                        ttBody = ttBody.Prototype as Body;
                    foreach (TaggedObject t0 in this.Selection1.GetSelectedObjects())
                    {
                        Body toBody = t0 as Body;
                        if (toBody.Prototype != null)
                            toBody = toBody.Prototype as Body;
                        ComputeDischargeFace cf = new ComputeDischargeFace(toBody, ttBody, mat, csys);
                        cf.GetBodyInfoForInterference(true);
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
                if (block == Selection)
                {
                    //---------Enter your code here-----------

                }
                else if (block == Selection1)
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