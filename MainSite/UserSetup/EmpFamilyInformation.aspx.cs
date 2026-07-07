using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.COMMON_DAL;
using DAL.UserPermissions_DAL;
using DAO.HRIS_DAO;
using DAO.HRIS_DAO_EF;
using HELPER_FUNCTIONS.HELPERS;

public partial class UserSetup_EmpFamilyInformation : System.Web.UI.Page
{
    private ShowMessage aShowMessage = new ShowMessage();
    private Messages aMessages = new Messages();
    private CommonDataLoadDAL _commonDataLoad = new CommonDataLoadDAL();
    private int mid = 0;
    private string _userId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] != "")
        {
            _userId = Convert.ToString(Session["UserId"].ToString());
        }
        if (!IsPostBack)
        {
            LoadDropDownList();

            txt_EmpSpouseDOB.Attributes.Add("readonly", "readonly");
            txt_EmpMarriageDate.Attributes.Add("readonly", "readonly");
            txt_EmpChildrenDOB.Attributes.Add("readonly", "readonly");




            if (!string.IsNullOrEmpty(Request.QueryString["mid"]))
            {
                mid = int.Parse(Request.QueryString["mid"]);
                hdpk.Value = mid.ToString();
                if (mid > 0)
                {
                    using (var db = new HRIS_SMCEntities())
                    {
                        var emp = (from j in db.tblEmpGeneralInfoes where j.EmpInfoId == mid select j).FirstOrDefault();

                        empMasterCode.Text =
                            emp.EmpMasterCode;
                        using (DataTable dtdesignation = _commonDataLoad.GetDTDesignationByEmpId(mid))
                        {
                            lblDesignation.Text = dtdesignation.Rows[0]["Designation"].ToString();

                        }
                        lblEmpName.Text = emp.EmpName;

                        #region 4. Family Information

                        txt_EmpSpouseName.Text = emp.SpouseName;
                        ddlEmpSpouseMaxEdu.SelectedValue = emp.SpouseMaxEducation.ToString();
                        ddlEmpSpouseOccupation.SelectedValue = emp.SpouseOccupation.ToString();


                        txt_EmpSpouseDOB.Text = string.IsNullOrEmpty(emp.SpouseDateOfBirth.ToString())
                            ? String.Empty
                            : emp.SpouseDateOfBirth.Value.ToString("dd-MMM-yyyy");
                        txt_EmpMarriageDate.Text = string.IsNullOrEmpty(emp.DateOfMarriage.ToString())
                            ? String.Empty
                            : emp.DateOfMarriage.Value.ToString("dd-MMM-yyyy");



                        //empMasterCode.Text = emp.EmpMasterCode;
                        //lbl_Mode.Text = "Mode:Update Information";

                        using (DataTable dtChildren = _commonDataLoad.GetDTEmpChildrenByEmpId(mid))
                        {
                            if (dtChildren.Rows.Count > 0)
                            {
                                ViewState["ChildrenTable"] = dtChildren;
                                gv_Children.DataSource = dtChildren;
                                gv_Children.DataBind();
                            }

                        }

                        #endregion end
                    }
                }
            }
        }
    }
    protected void homeButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("../DashBoard_UI/DashBoard.aspx");
    }


    private void LoadDropDownList()
    {
        using (DataTable dt = _commonDataLoad.GetDDLEducationName())
        {


            ddlEmpSpouseMaxEdu.DataSource = dt;
            ddlEmpSpouseMaxEdu.DataValueField = "Value";
            ddlEmpSpouseMaxEdu.DataTextField = "TextField";
            ddlEmpSpouseMaxEdu.DataBind();
        }

        using (DataTable dt = _commonDataLoad.GetDDLOccupation())
        {


            ddlEmpSpouseOccupation.DataSource = dt;
            ddlEmpSpouseOccupation.DataValueField = "Value";
            ddlEmpSpouseOccupation.DataTextField = "TextField";
            ddlEmpSpouseOccupation.DataBind();

            ddlEmpChildrenOccupation.DataSource = dt;
            ddlEmpChildrenOccupation.DataValueField = "Value";
            ddlEmpChildrenOccupation.DataTextField = "TextField";
            ddlEmpChildrenOccupation.DataBind();
            ////TODO bind other occupations

        }

    }

    protected void btnAddChildren_OnClick(object sender, EventArgs e)
    {
        AddNewToGrid_Children();
    }

    protected void lb_RemoveChildren_OnClick(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton) sender;
        GridViewRow gvRow = (GridViewRow) lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["ChildrenTable"] != null)
        {
            DataTable dt = (DataTable) ViewState["ChildrenTable"];
            dt.Rows.Remove(dt.Rows[rowID]);
            if (dt.Rows.Count > 0)
            {
                //Store the current data in ViewState for future reference  
                ViewState["ChildrenTable"] = dt;
                //Re bind the GridView for the updated data  
                gv_Children.DataSource = dt;
                gv_Children.DataBind();
            }
            else
            {
                ViewState["ChildrenTable"] = null;
                //Re bind the GridView for the updated data  
                gv_Children.DataSource = null;
                gv_Children.DataBind();
            }
        }
        //Set Previous Data on Postbacks  
        SetPreviousData_Children();
    }

    protected void lb_EditChildren_OnClick(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton) sender;
        GridViewRow row = (GridViewRow) lb.NamingContainer;
        if (row != null)
        {
            //gets the row index selected
            int index = row.RowIndex;

            //gets the datakey
            // int itemID = gv_Children.DataKeys(index).Value;

            //access row values and assign it to your TextBox
            //  txt_EmpChildrenName.Text = row.Cells[1].Text;
            //  ddlEmpChildrenGender.SelectedValue = row.Cells[2].Text;
            ////  ddlEmpChildrenOccupation.SelectedItem.Text = row.Cells[3].Text;
            //  txt_EmpChildrenDOB.Text = row.Cells[4].Text;


            //If you are using TemplateField then you can access them using FindControl() method

            txt_EmpChildrenName.Text = ((Label) row.FindControl("lbl_ChildrenName")).Text;
            ddlEmpChildrenGender.SelectedValue = ((Label) row.FindControl("lbl_ChildrenGender")).Text;

            ddlEmpChildrenOccupation.SelectedValue =
                string.IsNullOrEmpty(((HiddenField) row.FindControl("hfChildrenOccupation")).Value)
                    ? "-1"
                    : ((HiddenField) row.FindControl("hfChildrenOccupation")).Value;
            txt_EmpChildrenDOB.Text = ((Label) row.FindControl("lbl_ChildrenDOB")).Text;

            ddlChildrenMaritalStatus.SelectedValue = ((Label) row.FindControl("lbl_ChildrenMaritalStatus")).Text;







            GridViewRow gvRow = (GridViewRow) lb.NamingContainer;
            int rowID = gvRow.RowIndex;
            if (ViewState["ChildrenTable"] != null)
            {
                DataTable dt = (DataTable) ViewState["ChildrenTable"];
                dt.Rows.Remove(dt.Rows[rowID]);
                if (dt.Rows.Count > 0)
                {
                    //Store the current data in ViewState for future reference  
                    ViewState["ChildrenTable"] = dt;
                    //Re bind the GridView for the updated data  
                    gv_Children.DataSource = dt;
                    gv_Children.DataBind();
                }
                else
                {
                    ViewState["ChildrenTable"] = null;
                    //Re bind the GridView for the updated data  
                    gv_Children.DataSource = null;
                    gv_Children.DataBind();
                }
            }
            //Set Previous Data on Postbacks  
            SetPreviousData_Children();
        }
    }

    private void AddNewToGrid_Children()
    {
        if (ViewState["ChildrenTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable) ViewState["ChildrenTable"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {
                drCurrentRow = dtCurrentTable.NewRow();

                drCurrentRow["EmpChildrenId"] = DBNull.Value;
                drCurrentRow["EmpInfoId"] = hdpk.Value;
                drCurrentRow["ChildrenName"] = txt_EmpChildrenName.Text;
                drCurrentRow["ChildrenGender"] = ddlEmpChildrenGender.SelectedValue;
                if (ddlEmpChildrenOccupation.SelectedIndex > 0)
                {
                    drCurrentRow["ChildrenOccupation"] = ddlEmpChildrenOccupation.SelectedValue;
                    drCurrentRow["ChildrenOccupationName"] = ddlEmpChildrenOccupation.SelectedItem.Text;
                }

                drCurrentRow["ChildrenDOB"] = txt_EmpChildrenDOB.Text;
                drCurrentRow["ChildrenMaritalStatus"] = ddlChildrenMaritalStatus.SelectedValue;
                //add new row to DataTable   
                dtCurrentTable.Rows.Add(drCurrentRow);
                //Store the current data to ViewState for future reference   
                ViewState["ChildrenTable"] = dtCurrentTable;

                //Rebind the Grid with the current data to reflect changes   
                gv_Children.DataSource = dtCurrentTable;
                gv_Children.DataBind();
            }
        }
        else
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("EmpChildrenId", typeof (string)));
            dt.Columns.Add(new DataColumn("EmpInfoId", typeof (string)));
            dt.Columns.Add(new DataColumn("ChildrenName", typeof (string)));
            dt.Columns.Add(new DataColumn("ChildrenGender", typeof (string)));
            dt.Columns.Add(new DataColumn("ChildrenOccupation", typeof (string)));
            dt.Columns.Add(new DataColumn("ChildrenOccupationName", typeof (string)));
            dt.Columns.Add(new DataColumn("ChildrenDOB", typeof (string)));
            dt.Columns.Add(new DataColumn("ChildrenMaritalStatus", typeof (string)));

            dr = dt.NewRow();
            dr["EmpChildrenId"] = "";
            dr["EmpInfoId"] = hdpk.Value;
            dr["ChildrenName"] = txt_EmpChildrenName.Text;
            if (ddlEmpChildrenOccupation.SelectedIndex > 0)
            {
                dr["ChildrenOccupation"] = ddlEmpChildrenOccupation.SelectedValue;
                dr["ChildrenOccupationName"] = ddlEmpChildrenOccupation.SelectedItem.Text;
            }
            dr["ChildrenGender"] = ddlEmpChildrenGender.SelectedValue;
            dr["ChildrenDOB"] = txt_EmpChildrenDOB.Text;
            dr["ChildrenMaritalStatus"] = ddlChildrenMaritalStatus.SelectedValue;
            dt.Rows.Add(dr);

            //Store the DataTable in ViewState for future reference   
            ViewState["ChildrenTable"] = dt;

            //Bind the Gridview   
            gv_Children.DataSource = dt;
            gv_Children.DataBind();
        }
        //Set Previous Data on Postbacks   
        SetPreviousData_Children();
        txt_EmpChildrenName.Text = string.Empty;
        ddlEmpChildrenGender.SelectedValue = null;
        ddlEmpChildrenOccupation.SelectedValue = null;
        ddlEmpChildrenOccupation.SelectedValue = null;

        txt_EmpChildrenDOB.Text = string.Empty;
        ddlChildrenMaritalStatus.SelectedValue = string.Empty;

    }

    private void SetPreviousData_Children()
    {
        int rowIndex = 0;
        if (ViewState["ChildrenTable"] != null)
        {
            DataTable dt = (DataTable) ViewState["ChildrenTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    HiddenField EmpChildrenId = (HiddenField) gv_Children.Rows[rowIndex].FindControl("EmpChildrenId");
                    Label lbl_ChildrenName = (Label) gv_Children.Rows[rowIndex].FindControl("lbl_ChildrenName");
                    Label lbl_ChildrenGender = (Label) gv_Children.Rows[rowIndex].FindControl("lbl_ChildrenGender");
                    Label lbl_ChildrenOccupation =
                        (Label) gv_Children.Rows[rowIndex].FindControl("lbl_ChildrenOccupation");
                    HiddenField hfChildrenOccupation =
                        (HiddenField) gv_Children.Rows[rowIndex].FindControl("hfChildrenOccupation");
                    Label lbl_ChildrenDOB = (Label) gv_Children.Rows[rowIndex].FindControl("lbl_ChildrenDOB");
                    Label lbl_ChildrenMaritalStatus =
                        (Label) gv_Children.Rows[rowIndex].FindControl("lbl_ChildrenMaritalStatus");

                    if (i < dt.Rows.Count - 1)
                    {
                        EmpChildrenId.Value = dt.Rows[i]["EmpChildrenId"].ToString();
                        lbl_ChildrenName.Text = dt.Rows[i]["ChildrenName"].ToString();
                        lbl_ChildrenGender.Text = dt.Rows[i]["ChildrenGender"].ToString();
                        lbl_ChildrenOccupation.Text = dt.Rows[i]["ChildrenOccupationName"].ToString();
                        hfChildrenOccupation.Value = dt.Rows[i]["ChildrenOccupation"].ToString();
                        lbl_ChildrenDOB.Text = dt.Rows[i]["ChildrenDOB"].ToString();
                        lbl_ChildrenMaritalStatus.Text = dt.Rows[i]["ChildrenMaritalStatus"].ToString();
                    }

                    rowIndex++;
                }
            }
        }
    }

    protected void btn_Save_OnClick(object sender, EventArgs e)
    {
        

        //try
        //{
        //    string EmpMasterCode = string.Empty;
        //    mid = string.IsNullOrEmpty(hdpk.Value) ? 0 : int.Parse(hdpk.Value);
        //    tblEmpGeneralInfo emp = null;
        //    using (var db = new HRIS_SMCEntities())
        //    {
        //        if (mid > 0)
        //        {
        //            emp = (from j in db.tblEmpGeneralInfoes where j.EmpInfoId == mid select j).FirstOrDefault();


        //            emp.SpouseName = string.IsNullOrEmpty(txt_EmpSpouseName.Text) ? null : txt_EmpSpouseName.Text;
        //            emp.SpouseMaxEducation = ddlEmpSpouseMaxEdu.SelectedIndex > 0
        //                ? int.Parse(ddlEmpSpouseMaxEdu.SelectedValue)
        //                : (int?) null;
        //            emp.SpouseOccupation = ddlEmpSpouseOccupation.SelectedIndex > 0
        //                ? int.Parse(ddlEmpSpouseOccupation.SelectedValue)
        //                : (int?) null;
        //            emp.SpouseDateOfBirth = string.IsNullOrEmpty(txt_EmpSpouseDOB.Text)
        //                ? (DateTime?) null
        //                : DateTime.Parse(txt_EmpSpouseDOB.Text).Date;

        //            emp.DateOfMarriage = string.IsNullOrEmpty(txt_EmpMarriageDate.Text)
        //                ? (DateTime?) null
        //                : DateTime.Parse(txt_EmpMarriageDate.Text).Date;


        //            db.SaveChanges();


        //            if (gv_Children.Rows.Count == 0)
        //            {
        //                db.Database.ExecuteSqlCommand("UPDATE dbo.tblEmpChildren SET IsActive=0 WHERE EmpInfoId={0}",
        //                    emp.EmpInfoId);
        //            }

        //            if (gv_Children.Rows.Count > 0)
        //            {
        //                //making previous entris inactive

        //                db.Database.ExecuteSqlCommand("UPDATE dbo.tblEmpChildren SET IsActive=0 WHERE EmpInfoId={0}",
        //                    emp.EmpInfoId);
        //                for (int i = 0; i < gv_Children.Rows.Count; i++)
        //                {
        //                    HiddenField EmpChildrenId = (HiddenField) gv_Children.Rows[i].FindControl("EmpChildrenId");
        //                    Label lbl_ChildrenName = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenName");
        //                    Label lbl_ChildrenGender = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenGender");
        //                    HiddenField hfChildrenOccupation =
        //                        (HiddenField) gv_Children.Rows[i].FindControl("hfChildrenOccupation");
        //                    Label lbl_ChildrenDOB = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenDOB");
        //                    Label lbl_ChildrenMaritalStatus =
        //                        (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenMaritalStatus");

        //                    if (string.IsNullOrEmpty(EmpChildrenId.Value))
        //                    {
        //                        tblEmpChildren children = new tblEmpChildren();

        //                        children.EmpInfoId = emp.EmpInfoId;
        //                        children.ChildrenName = string.IsNullOrEmpty(lbl_ChildrenName.Text) ? null  : lbl_ChildrenName.Text;
        //                        children.ChildrenGender = string.IsNullOrEmpty(lbl_ChildrenGender.Text)
        //                            ? null
        //                            : lbl_ChildrenGender.Text;
        //                        children.ChildrenOccupation = string.IsNullOrEmpty(hfChildrenOccupation.Value) ? (int?) null : int.Parse(hfChildrenOccupation.Value);
        //                        children.ChildrenDOB = string.IsNullOrEmpty(lbl_ChildrenDOB.Text)
        //                            ? (DateTime?) null
        //                            : DateTime.Parse(lbl_ChildrenDOB.Text).Date;
        //                        children.ChildrenMaritalStatus = string.IsNullOrEmpty(lbl_ChildrenMaritalStatus.Text)
        //                            ? null
        //                            : lbl_ChildrenMaritalStatus.Text;
        //                        children.IsActive = true;
        //                        db.tblEmpChildrens.Add(children);
        //                    }
        //                    else
        //                    {
        //                        int u_EmpChildrenId = int.Parse(EmpChildrenId.Value);
        //                        tblEmpChildren children =
        //                            (from j in db.tblEmpChildrens where j.EmpChildrenId == u_EmpChildrenId select j)
        //                                .FirstOrDefault();

        //                        children.EmpInfoId = emp.EmpInfoId;
        //                        children.ChildrenName = string.IsNullOrEmpty(lbl_ChildrenName.Text)
        //                            ? null
        //                            : lbl_ChildrenName.Text;
        //                        children.ChildrenGender = string.IsNullOrEmpty(lbl_ChildrenGender.Text)
        //                            ? null
        //                            : lbl_ChildrenGender.Text;
        //                        children.ChildrenOccupation = string.IsNullOrEmpty(hfChildrenOccupation.Value)
        //                            ? (int?) null
        //                            : int.Parse(hfChildrenOccupation.Value);
        //                        children.ChildrenDOB = string.IsNullOrEmpty(lbl_ChildrenDOB.Text)
        //                            ? (DateTime?) null
        //                            : DateTime.Parse(lbl_ChildrenDOB.Text).Date;
        //                        children.ChildrenMaritalStatus = string.IsNullOrEmpty(lbl_ChildrenMaritalStatus.Text)
        //                            ? null
        //                            : lbl_ChildrenMaritalStatus.Text;
        //                        children.IsActive = true;
        //                    }
        //                    db.SaveChanges();
        //                }
        //            } ////End Childrens

        //            else
        //            {


        //                //emp.EmpMasterCode = "EM" + (1000+emp.EmpInfoId);

        //                if (gv_Children.Rows.Count == 0)
        //                {
        //                    db.Database.ExecuteSqlCommand(
        //                        "UPDATE dbo.tblEmpChildren SET IsActive=0 WHERE EmpInfoId={0}", emp.EmpInfoId);
        //                }

        //                if (gv_Children.Rows.Count > 0)
        //                {
        //                    for (int i = 0; i < gv_Children.Rows.Count; i++)
        //                    {
        //                        HiddenField EmpChildrenId =
        //                            (HiddenField) gv_Children.Rows[i].FindControl("EmpChildrenId");
        //                        Label lbl_ChildrenName = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenName");
        //                        Label lbl_ChildrenGender = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenGender");
        //                        HiddenField hfChildrenOccupation =
        //                            (HiddenField) gv_Children.Rows[i].FindControl("hfChildrenOccupation");
        //                        Label lbl_ChildrenDOB = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenDOB");
        //                        Label lbl_ChildrenMaritalStatus =
        //                            (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenMaritalStatus");
        //                        tblEmpChildren children = new tblEmpChildren();

        //                        children.EmpInfoId = emp.EmpInfoId;
        //                        children.ChildrenName = string.IsNullOrEmpty(lbl_ChildrenName.Text)
        //                            ? null
        //                            : lbl_ChildrenName.Text;
        //                        children.ChildrenGender = string.IsNullOrEmpty(lbl_ChildrenGender.Text)
        //                            ? null
        //                            : lbl_ChildrenGender.Text;
        //                        children.ChildrenOccupation = string.IsNullOrEmpty(hfChildrenOccupation.Value)
        //                            ? (int?) null
        //                            : int.Parse(hfChildrenOccupation.Value);
        //                        children.ChildrenDOB = string.IsNullOrEmpty(lbl_ChildrenDOB.Text)
        //                            ? (DateTime?) null
        //                            : DateTime.Parse(lbl_ChildrenDOB.Text).Date;
        //                        children.ChildrenMaritalStatus = string.IsNullOrEmpty(lbl_ChildrenMaritalStatus.Text)
        //                            ? null
        //                            : lbl_ChildrenMaritalStatus.Text;
        //                        children.IsActive = true;
        //                        db.tblEmpChildrens.Add(children);
        //                    }
        //                } ////End Childrens
        //                EmployeeInfoListReportDAL _empdal = new EmployeeInfoListReportDAL();
        //                ////Below stored procedure will generate Emp Master Code based on condition, update on database and return the value
        //                using (DataTable dtEmpCode = _empdal.GetEmpMasterCode(emp.EmpInfoId))
        //                {
        //                    if (dtEmpCode.Rows.Count > 0)
        //                    {
        //                        EmpMasterCode = dtEmpCode.Rows[0]["EmpMasterCode"].ToString();
        //                    }

        //                }

                       

        //            }



        //        }
        //    }
        //    ScriptManager.RegisterStartupScript(this, this.GetType(),
        //        "alert",
        //        "alert('Operation Successful...! ');window.location ='EmployeeInfoList.aspx';",
        //        true);
        //}
        //catch (Exception ex)
        //{
        //    AlertMessageBoxShow(ex.Message);
        //}
        try
        {
            string MasterId = string.Empty;
            string EmpMasterCode = string.Empty;
            mid = string.IsNullOrEmpty(hdpk.Value) ? 0 : int.Parse(hdpk.Value);
            tblEmpGeneralInfo emp = null;
            using (var db = new HRIS_SMCEntities())
            {
                if (mid > 0)
                {
                    emp = (from j in db.tblEmpGeneralInfoes where j.EmpInfoId == mid select j).FirstOrDefault();
                    MasterId = emp.EmpInfoId.ToString();

                    emp.SpouseName = string.IsNullOrEmpty(txt_EmpSpouseName.Text) ? null : txt_EmpSpouseName.Text;
                    emp.SpouseMaxEducation = ddlEmpSpouseMaxEdu.SelectedIndex > 0
                        ? int.Parse(ddlEmpSpouseMaxEdu.SelectedValue)
                        : (int?)null;
                    emp.SpouseOccupation = ddlEmpSpouseOccupation.SelectedIndex > 0
                        ? int.Parse(ddlEmpSpouseOccupation.SelectedValue)
                        : (int?)null;
                    emp.SpouseDateOfBirth = string.IsNullOrEmpty(txt_EmpSpouseDOB.Text)
                        ? (DateTime?)null
                        : DateTime.Parse(txt_EmpSpouseDOB.Text).Date;

                    emp.DateOfMarriage = string.IsNullOrEmpty(txt_EmpMarriageDate.Text)
                        ? (DateTime?)null
                        : DateTime.Parse(txt_EmpMarriageDate.Text).Date;


                    db.SaveChanges();


                    if (gv_Children.Rows.Count == 0)
                    {
                        db.Database.ExecuteSqlCommand("UPDATE dbo.tblEmpChildren SET IsActive=0 WHERE EmpInfoId={0}",
                            emp.EmpInfoId);
                    }

                    if (gv_Children.Rows.Count > 0)
                    {
                        //making previous entris inactive

                        db.Database.ExecuteSqlCommand("UPDATE dbo.tblEmpChildren SET IsActive=0 WHERE EmpInfoId={0}",
                            emp.EmpInfoId);
                        for (int i = 0; i < gv_Children.Rows.Count; i++)
                        {
                            HiddenField EmpChildrenId = (HiddenField)gv_Children.Rows[i].FindControl("EmpChildrenId");
                            Label lbl_ChildrenName = (Label)gv_Children.Rows[i].FindControl("lbl_ChildrenName");
                            Label lbl_ChildrenGender = (Label)gv_Children.Rows[i].FindControl("lbl_ChildrenGender");
                            HiddenField hfChildrenOccupation =
                                (HiddenField)gv_Children.Rows[i].FindControl("hfChildrenOccupation");
                            Label lbl_ChildrenDOB = (Label)gv_Children.Rows[i].FindControl("lbl_ChildrenDOB");
                            Label lbl_ChildrenMaritalStatus =
                                (Label)gv_Children.Rows[i].FindControl("lbl_ChildrenMaritalStatus");

                            if (string.IsNullOrEmpty(EmpChildrenId.Value))
                            {
                                tblEmpChildren children = new tblEmpChildren();

                                children.EmpInfoId = emp.EmpInfoId;
                                children.ChildrenName = string.IsNullOrEmpty(lbl_ChildrenName.Text)
                                    ? null
                                    : lbl_ChildrenName.Text;
                                children.ChildrenGender = string.IsNullOrEmpty(lbl_ChildrenGender.Text)
                                    ? null
                                    : lbl_ChildrenGender.Text;
                                children.ChildrenOccupation = string.IsNullOrEmpty(hfChildrenOccupation.Value)
                                    ? (int?)null
                                    : int.Parse(hfChildrenOccupation.Value);
                                children.ChildrenDOB = string.IsNullOrEmpty(lbl_ChildrenDOB.Text)
                                    ? (DateTime?)null
                                    : DateTime.Parse(lbl_ChildrenDOB.Text).Date;
                                children.ChildrenMaritalStatus = string.IsNullOrEmpty(lbl_ChildrenMaritalStatus.Text)
                                    ? null
                                    : lbl_ChildrenMaritalStatus.Text;
                                children.IsActive = true;
                                db.tblEmpChildrens.Add(children);
                            }
                            else
                            {
                                int u_EmpChildrenId = int.Parse(EmpChildrenId.Value);
                                tblEmpChildren children =
                                    (from j in db.tblEmpChildrens where j.EmpChildrenId == u_EmpChildrenId select j)
                                        .FirstOrDefault();

                                children.EmpInfoId = emp.EmpInfoId;
                                children.ChildrenName = string.IsNullOrEmpty(lbl_ChildrenName.Text)
                                    ? null
                                    : lbl_ChildrenName.Text;
                                children.ChildrenGender = string.IsNullOrEmpty(lbl_ChildrenGender.Text)
                                    ? null
                                    : lbl_ChildrenGender.Text;
                                children.ChildrenOccupation = string.IsNullOrEmpty(hfChildrenOccupation.Value)
                                    ? (int?)null
                                    : int.Parse(hfChildrenOccupation.Value);
                                children.ChildrenDOB = string.IsNullOrEmpty(lbl_ChildrenDOB.Text)
                                    ? (DateTime?)null
                                    : DateTime.Parse(lbl_ChildrenDOB.Text).Date;
                                children.ChildrenMaritalStatus = string.IsNullOrEmpty(lbl_ChildrenMaritalStatus.Text)
                                    ? null
                                    : lbl_ChildrenMaritalStatus.Text;
                                children.IsActive = true;
                            }
                            db.SaveChanges();
                        }
                    } ////End Childrens
                }
                else
                {

                    //MasterId = emp.EmpInfoId.ToString();
                    ////emp.EmpMasterCode = "EM" + (1000+emp.EmpInfoId);

                    //if (gv_Children.Rows.Count == 0)
                    //{
                    //    db.Database.ExecuteSqlCommand("UPDATE dbo.tblEmpChildren SET IsActive=0 WHERE EmpInfoId={0}",
                    //        emp.EmpInfoId);
                    //}
                    //emp.EmpInfoId = Convert.ToInt64(MasterId);
                    //if (gv_Children.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < gv_Children.Rows.Count; i++)
                    //    {
                    //        HiddenField EmpChildrenId =
                    //            (HiddenField) gv_Children.Rows[i].FindControl("EmpChildrenId");
                    //        Label lbl_ChildrenName = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenName");
                    //        Label lbl_ChildrenGender = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenGender");
                    //        HiddenField hfChildrenOccupation =
                    //            (HiddenField) gv_Children.Rows[i].FindControl("hfChildrenOccupation");
                    //        Label lbl_ChildrenDOB = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenDOB");
                    //        Label lbl_ChildrenMaritalStatus =
                    //            (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenMaritalStatus");
                    //        tblEmpChildren children = new tblEmpChildren();

                    //        children.EmpInfoId = emp.EmpInfoId;
                    //        children.ChildrenName = string.IsNullOrEmpty(lbl_ChildrenName.Text)
                    //            ? null
                    //            : lbl_ChildrenName.Text;
                    //        children.ChildrenGender = string.IsNullOrEmpty(lbl_ChildrenGender.Text)
                    //            ? null
                    //            : lbl_ChildrenGender.Text;
                    //        children.ChildrenOccupation = string.IsNullOrEmpty(hfChildrenOccupation.Value)
                    //            ? (int?) null
                    //            : int.Parse(hfChildrenOccupation.Value);
                    //        children.ChildrenDOB = string.IsNullOrEmpty(lbl_ChildrenDOB.Text)
                    //            ? (DateTime?) null
                    //            : DateTime.Parse(lbl_ChildrenDOB.Text).Date;
                    //        children.ChildrenMaritalStatus = string.IsNullOrEmpty(lbl_ChildrenMaritalStatus.Text)
                    //            ? null
                    //            : lbl_ChildrenMaritalStatus.Text;
                    //        children.IsActive = true;
                    //        db.tblEmpChildrens.Add(children);
                    //    }
                    //} ////End Childrens


                    //EmployeeInfoListReportDAL _empdal = new EmployeeInfoListReportDAL();
                    //////Below stored procedure will generate Emp Master Code based on condition, update on database and return the value
                    //using (DataTable dtEmpCode = _empdal.GetEmpMasterCode(emp.EmpInfoId))
                    //{
                    //    if (dtEmpCode.Rows.Count > 0)
                    //    {
                    //        EmpMasterCode = dtEmpCode.Rows[0]["EmpMasterCode"].ToString();
                    //    }

                    //}



                }


            }
            //  ScriptManager.RegisterStartupScript(this, this.GetType(),
            //"alert",
            //"alert('Operation Successful...!);window.location ='EmpEducation.aspx?mid=" + MasterId + "';",
            //true);
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "alert",
                "alert('Operation Successful...! ');window.location ='EmployeeInfoList.aspx';",
                true);
          
        }
        catch (Exception ex)
        {
            AlertMessageBoxShow(ex.Message);
        }

    }
    protected void btnEditInfo_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeInfoList.aspx");
    }
    private void AlertMessageBoxShow(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

    }

    protected void btn_Next_OnClick(object sender, EventArgs e)
    {
        try
        {
            string MasterId = string.Empty;
            string EmpMasterCode = string.Empty;
            mid = string.IsNullOrEmpty(hdpk.Value) ? 0 : int.Parse(hdpk.Value);
            tblEmpGeneralInfo emp = null;
            using (var db = new HRIS_SMCEntities())
            {
                if (mid > 0)
                {
                    emp = (from j in db.tblEmpGeneralInfoes where j.EmpInfoId == mid select j).FirstOrDefault();
                    MasterId = emp.EmpInfoId.ToString();

                    emp.SpouseName = string.IsNullOrEmpty(txt_EmpSpouseName.Text) ? null : txt_EmpSpouseName.Text;
                    emp.SpouseMaxEducation = ddlEmpSpouseMaxEdu.SelectedIndex > 0
                        ? int.Parse(ddlEmpSpouseMaxEdu.SelectedValue)
                        : (int?) null;
                    emp.SpouseOccupation = ddlEmpSpouseOccupation.SelectedIndex > 0
                        ? int.Parse(ddlEmpSpouseOccupation.SelectedValue)
                        : (int?) null;
                    emp.SpouseDateOfBirth = string.IsNullOrEmpty(txt_EmpSpouseDOB.Text)
                        ? (DateTime?) null
                        : DateTime.Parse(txt_EmpSpouseDOB.Text).Date;

                    emp.DateOfMarriage = string.IsNullOrEmpty(txt_EmpMarriageDate.Text)
                        ? (DateTime?) null
                        : DateTime.Parse(txt_EmpMarriageDate.Text).Date;


                    db.SaveChanges();


                    if (gv_Children.Rows.Count == 0)
                    {
                        db.Database.ExecuteSqlCommand("UPDATE dbo.tblEmpChildren SET IsActive=0 WHERE EmpInfoId={0}",
                            emp.EmpInfoId);
                    }

                    if (gv_Children.Rows.Count > 0)
                    {
                        //making previous entris inactive

                        db.Database.ExecuteSqlCommand("UPDATE dbo.tblEmpChildren SET IsActive=0 WHERE EmpInfoId={0}",
                            emp.EmpInfoId);
                        for (int i = 0; i < gv_Children.Rows.Count; i++)
                        {
                            HiddenField EmpChildrenId = (HiddenField) gv_Children.Rows[i].FindControl("EmpChildrenId");
                            Label lbl_ChildrenName = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenName");
                            Label lbl_ChildrenGender = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenGender");
                            HiddenField hfChildrenOccupation =
                                (HiddenField) gv_Children.Rows[i].FindControl("hfChildrenOccupation");
                            Label lbl_ChildrenDOB = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenDOB");
                            Label lbl_ChildrenMaritalStatus =
                                (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenMaritalStatus");

                            if (string.IsNullOrEmpty(EmpChildrenId.Value))
                            {
                                tblEmpChildren children = new tblEmpChildren();

                                children.EmpInfoId = emp.EmpInfoId;
                                children.ChildrenName = string.IsNullOrEmpty(lbl_ChildrenName.Text)
                                    ? null
                                    : lbl_ChildrenName.Text;
                                children.ChildrenGender = string.IsNullOrEmpty(lbl_ChildrenGender.Text)
                                    ? null
                                    : lbl_ChildrenGender.Text;
                                children.ChildrenOccupation = string.IsNullOrEmpty(hfChildrenOccupation.Value)
                                    ? (int?) null
                                    : int.Parse(hfChildrenOccupation.Value);
                                children.ChildrenDOB = string.IsNullOrEmpty(lbl_ChildrenDOB.Text)
                                    ? (DateTime?) null
                                    : DateTime.Parse(lbl_ChildrenDOB.Text).Date;
                                children.ChildrenMaritalStatus = string.IsNullOrEmpty(lbl_ChildrenMaritalStatus.Text)
                                    ? null
                                    : lbl_ChildrenMaritalStatus.Text;
                                children.IsActive = true;
                                db.tblEmpChildrens.Add(children);
                            }
                            else
                            {
                                int u_EmpChildrenId = int.Parse(EmpChildrenId.Value);
                                tblEmpChildren children =
                                    (from j in db.tblEmpChildrens where j.EmpChildrenId == u_EmpChildrenId select j)
                                        .FirstOrDefault();

                                children.EmpInfoId = emp.EmpInfoId;
                                children.ChildrenName = string.IsNullOrEmpty(lbl_ChildrenName.Text)
                                    ? null
                                    : lbl_ChildrenName.Text;
                                children.ChildrenGender = string.IsNullOrEmpty(lbl_ChildrenGender.Text)
                                    ? null
                                    : lbl_ChildrenGender.Text;
                                children.ChildrenOccupation = string.IsNullOrEmpty(hfChildrenOccupation.Value)
                                    ? (int?) null
                                    : int.Parse(hfChildrenOccupation.Value);
                                children.ChildrenDOB = string.IsNullOrEmpty(lbl_ChildrenDOB.Text)
                                    ? (DateTime?) null
                                    : DateTime.Parse(lbl_ChildrenDOB.Text).Date;
                                children.ChildrenMaritalStatus = string.IsNullOrEmpty(lbl_ChildrenMaritalStatus.Text)
                                    ? null
                                    : lbl_ChildrenMaritalStatus.Text;
                                children.IsActive = true;
                            }
                            db.SaveChanges();
                        }
                    } ////End Childrens
                }
                else
                {

                    //MasterId = emp.EmpInfoId.ToString();
                    ////emp.EmpMasterCode = "EM" + (1000+emp.EmpInfoId);

                    //if (gv_Children.Rows.Count == 0)
                    //{
                    //    db.Database.ExecuteSqlCommand("UPDATE dbo.tblEmpChildren SET IsActive=0 WHERE EmpInfoId={0}",
                    //        emp.EmpInfoId);
                    //}
                    //emp.EmpInfoId = Convert.ToInt64(MasterId);
                    //if (gv_Children.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < gv_Children.Rows.Count; i++)
                    //    {
                    //        HiddenField EmpChildrenId =
                    //            (HiddenField) gv_Children.Rows[i].FindControl("EmpChildrenId");
                    //        Label lbl_ChildrenName = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenName");
                    //        Label lbl_ChildrenGender = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenGender");
                    //        HiddenField hfChildrenOccupation =
                    //            (HiddenField) gv_Children.Rows[i].FindControl("hfChildrenOccupation");
                    //        Label lbl_ChildrenDOB = (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenDOB");
                    //        Label lbl_ChildrenMaritalStatus =
                    //            (Label) gv_Children.Rows[i].FindControl("lbl_ChildrenMaritalStatus");
                    //        tblEmpChildren children = new tblEmpChildren();

                    //        children.EmpInfoId = emp.EmpInfoId;
                    //        children.ChildrenName = string.IsNullOrEmpty(lbl_ChildrenName.Text)
                    //            ? null
                    //            : lbl_ChildrenName.Text;
                    //        children.ChildrenGender = string.IsNullOrEmpty(lbl_ChildrenGender.Text)
                    //            ? null
                    //            : lbl_ChildrenGender.Text;
                    //        children.ChildrenOccupation = string.IsNullOrEmpty(hfChildrenOccupation.Value)
                    //            ? (int?) null
                    //            : int.Parse(hfChildrenOccupation.Value);
                    //        children.ChildrenDOB = string.IsNullOrEmpty(lbl_ChildrenDOB.Text)
                    //            ? (DateTime?) null
                    //            : DateTime.Parse(lbl_ChildrenDOB.Text).Date;
                    //        children.ChildrenMaritalStatus = string.IsNullOrEmpty(lbl_ChildrenMaritalStatus.Text)
                    //            ? null
                    //            : lbl_ChildrenMaritalStatus.Text;
                    //        children.IsActive = true;
                    //        db.tblEmpChildrens.Add(children);
                    //    }
                    //} ////End Childrens

                    
                    //EmployeeInfoListReportDAL _empdal = new EmployeeInfoListReportDAL();
                    //////Below stored procedure will generate Emp Master Code based on condition, update on database and return the value
                    //using (DataTable dtEmpCode = _empdal.GetEmpMasterCode(emp.EmpInfoId))
                    //{
                    //    if (dtEmpCode.Rows.Count > 0)
                    //    {
                    //        EmpMasterCode = dtEmpCode.Rows[0]["EmpMasterCode"].ToString();
                    //    }

                    //}

                   

                }
                string loc = "EmpEducation.aspx?mid=" + MasterId;
                ScriptManager.RegisterStartupScript(this, this.GetType(),
               "alert",
               "alert('Operation Successful...! ');window.location ='" + loc + "';",
               true);
                //AlertMessageBoxShow("Operation Successful...!");
                //Response.Redirect("EmpEducation.aspx?mid=" + MasterId);
            }
          //  ScriptManager.RegisterStartupScript(this, this.GetType(),
          //"alert",
          //"alert('Operation Successful.);window.location ='EmpEducation.aspx?mid=" + MasterId + "';",
          //true);
         
        }
        catch (Exception ex)
        {
          //  AlertMessageBoxShow(ex.Message);
        }
}

    protected void cancelButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeInfoList.aspx");
    }

    protected void detailsViewButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeInfoListUpdate.aspx");
    }

    protected void lblNext_OnClick(object sender, EventArgs e)
    {
        string EmpId = Request.QueryString["mid"];
        if (Convert.ToInt32(EmpId) > 0)
        {
            Response.Redirect("EmpEducation?mid=" + EmpId);
        }
        else
        {
            Response.Redirect("EmployeeInfoListUpdate.aspx");
        }
    }

    protected void lbPrevious_OnClick(object sender, EventArgs e)
    {
        string EmpId = Request.QueryString["mid"];
        if (Convert.ToInt32(EmpId) > 0)
        {
            Response.Redirect("EmpContacts?mid=" + EmpId);
        }
        else
        {
            Response.Redirect("EmployeeInfoListUpdate.aspx");
        }
    }
}