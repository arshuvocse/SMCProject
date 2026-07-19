<%@ page title="" language="C#" masterpagefile="~/MasterPages/MainMasterPage.master" autoeventwireup="true" inherits="MeetingMinors_MeetingStatusList, App_Web_ce25useu" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=16.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    
         <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js" integrity="sha512-rMGGF4wg1R73ehtnxXBt5mbUfN9JUJwbk21KMlnLZDJh7BkPmeovBuddZCENJddHYYMkCh9hPFnPmS9sspki8g==" crossorigin="anonymous"></script>
    
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" integrity="sha512-yVvxUQV0QESBt1SyZbNJMAwyKvFTLMyXSyBHDO4BG5t7k/Lw34tyqlSDlKIrIENIzCl+RVUNjmCPG+V/GMesRw==" crossorigin="anonymous" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpFormBody" Runat="Server">
      <style>
        fieldset.for-panel {
          
            padding: 10px 8px;
            background-color: white;
            margin-bottom: 12px;
        }

            fieldset.for-panel legend {
               
                border-radius: 1px;
                font-size: 12px;
                font-weight: bold;
                line-height: 10px;
                margin: inherit;
                padding: 7px;
                width: auto;
                margin-bottom: 0;
                color: black;
            }

  
  .chkChoice label {
            padding-left: 4px;
            padding-right: 4px;
        }

  
.title-widget {
	color: #898989;
	font-size: 20px;
	font-weight: 300;
	line-height: 1;
	position: relative;
	text-transform: uppercase;
	font-family: 'Fjalla One', sans-serif;
	margin-top: 0;
	margin-right: 0;
	margin-bottom: 25px;
	 
	padding-left: 12px;

}

.title-widget::before {
    background-color: #ea5644;
    content: "";
    height: 22px;
    left: 0px;
    position: absolute;
    top: -2px;
    width: 5px;
}

.meeting-search-panel .form-group {
    margin-bottom: 10px;
}

.meeting-search-panel label {
    font-weight: 600;
    margin-bottom: 4px;
}

.search-section-selector {
    display: inline-flex;
    background-color: #f1f3f5;
    padding: 6px;
    border-radius: 50px;
    border: 1px solid #e9ecef;
    box-shadow: inset 0 2px 4px rgba(0, 0, 0, 0.04);
    margin-bottom: 15px;
}

.search-section-selector input[type="radio"] {
    position: absolute;
    opacity: 0;
    width: 0;
    height: 0;
    pointer-events: none;
}

.search-section-selector label {
    display: inline-block;
    padding: 8px 24px;
    margin: 0 2px !important;
    font-size: 14px;
    font-weight: 500;
    color: #495057;
    cursor: pointer;
    border-radius: 50px;
    transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
    user-select: none;
    text-align: center;
}

.search-section-selector label:hover {
    color: #212529;
    background-color: rgba(0, 0, 0, 0.05);
}

.search-section-selector input[type="radio"]:checked + label {
    background-color: #5B799E;
    color: #ffffff;
    font-weight: 600;
    box-shadow: 0 4px 6px rgba(91, 121, 158, 0.25);
}

/* Key Search Style Enhancements */
.key-search-box {
    position: relative;
    background: #ffffff;
    border: 1px solid #ced4da;
    border-radius: 8px;
    padding: 20px 24px;
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05), 0 2px 4px -1px rgba(0, 0, 0, 0.03);
    margin-top: 15px;
    margin-bottom: 20px;
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
    border-top: 3px solid #5B799E; /* Primary color indicator */
}

.key-search-box:hover {
    border-color: #5B799E;
    box-shadow: 0 10px 15px -3px rgba(91, 121, 158, 0.12), 0 4px 6px -2px rgba(91, 121, 158, 0.05);
}

.key-search-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 12px;
}

.key-search-title {
    font-size: 13px !important;
    font-weight: 700 !important;
    color: #2d3748 !important;
    margin-bottom: 0 !important;
    text-transform: uppercase;
    letter-spacing: 0.5px;
    display: flex;
    align-items: center;
}

.key-search-title i {
    color: #5B799E;
    font-size: 14px;
}

.key-search-badge {
    background-color: #ebf8ff;
    color: #2b6cb0;
    border: 1px solid #bee3f8;
    font-size: 11px;
    font-weight: 600;
    padding: 3px 8px;
    border-radius: 6px;
    display: flex;
    align-items: center;
}

.key-search-badge i {
    font-size: 11px;
}

.key-search-input-wrapper {
    position: relative;
    display: flex;
    align-items: center;
}

.key-search-input-wrapper .search-icon {
    position: absolute;
    left: 14px;
    color: #a0aec0;
    font-size: 14px;
    transition: color 0.2s ease;
    pointer-events: none;
}

.key-search-input-wrapper .search-textbox {
    padding-left: 40px !important;
    padding-right: 95px !important;
    height: 42px !important;
    border-radius: 6px !important;
    border: 1px solid #cbd5e0 !important;
    font-size: 13.5px !important;
    transition: all 0.2s ease !important;
    box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.05) !important;
}

.key-search-input-wrapper .search-textbox:focus {
    border-color: #5B799E !important;
    box-shadow: 0 0 0 3px rgba(91, 121, 158, 0.15) !important;
    background-color: #fff !important;
}

.key-search-input-wrapper .search-textbox:focus ~ .search-icon {
    color: #5B799E;
}

.key-search-input-wrapper .file-types-hint {
    position: absolute;
    right: 14px;
    display: flex;
    align-items: center;
    gap: 6px;
    pointer-events: none;
}

.file-types-hint span {
    font-size: 11px;
    font-weight: 600;
    padding: 2px 6px;
    border-radius: 4px;
    text-transform: uppercase;
}

.file-types-hint .badge-pdf {
    background-color: #fff5f5;
    color: #e53e3e;
    border: 1px solid #fed7d7;
}

.file-types-hint .badge-img {
    background-color: #f0fff4;
    color: #38a169;
    border: 1px solid #c6f6d5;
}

.key-search-footer-hint {
    margin-top: 8px;
    font-size: 11.5px;
    color: #718096;
    display: flex;
    align-items: center;
}

.key-search-footer-hint i {
    font-size: 12px;
}
          </style>
    
     <div class="content" id="content">
                 <div class="container-fluid">
                    <div class="page-heading">
                        <div class="page-heading__container">
                             
                            <h1 class="title" style="font-size: 18px; padding-top: 0px;"><img src="../Report_Pages/app.png"  width="20px" />  Meeting </h1>
                        </div>
                         <div class="page-heading__container float-right d-none d-sm-block">
                            <style>
                                .btn-custom-add {
                                    background: linear-gradient(135deg, #5B799E 0%, #3a5372 100%);
                                    color: #ffffff !important;
                                    border: none;
                                    padding: 10px 24px;
                                    font-size: 15px;
                                    font-weight: 600;
                                    border-radius: 6px;
                                    box-shadow: 0 4px 6px rgba(91, 121, 158, 0.3);
                                    transition: all 0.3s ease;
                                    text-transform: uppercase;
                                    letter-spacing: 0.5px;
                                }

                                .btn-custom-add:hover {
                                    background: linear-gradient(135deg, #4a6586 0%, #2c425d 100%);
                                    box-shadow: 0 6px 12px rgba(91, 121, 158, 0.4);
                                    transform: translateY(-2px);
                                    color: #ffffff !important;
                                }
                            </style>
                <asp:Button ID="AddNewButton" Text="Add New" CssClass="btn-custom-add" runat="server" OnClick="AddNewButton_OnClick"  />
                <%--<asp:Button ID="reloadButton" Text="Reload" CssClass="btn btn-sm btn-outline-success" runat="server" OnClick="reloadButton_OnClick" />--%>
            </div>
                    </div>
                     
<asp:UpdatePanel runat="server">
    <ContentTemplate>
         <asp:UpdateProgress ID="UpdateProgress4" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                    <ProgressTemplate>
                        <div class="divWaiting">
                            <asp:Image ID="imgWait11" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/Assets/img/progress-bar-opt.gif" Width="120px" Height="120px" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
        <div class="card">
   <div class="card-body">
       
          <div class="row mb-3" style="display:none;">
              <div class="col-md-12 text-center">
                  <asp:RadioButtonList runat="server" ID="rblSearchSection" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="search-section-selector">
                      <asp:ListItem Value="All" Selected="True">All</asp:ListItem>
                      <asp:ListItem Value="Meeting">Meeting</asp:ListItem>
                      <asp:ListItem Value="Agenda">Agenda</asp:ListItem>
                      <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                      <asp:ListItem Value="Approval">Approval</asp:ListItem>
                  </asp:RadioButtonList>
              </div>
          </div>

          <div class="row meeting-search-panel">
              <div class="col-md-3">
                  <div class="form-group">
                      <label>Company</label>
                      <asp:DropDownList runat="server" ID="ddlCompany" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_OnSelectedIndexChanged" CssClass="form-control form-control-sm" />
                  </div>
              </div>
              <div class="col-md-3 filter-group filter-meeting">
                  <div class="form-group">
                      <label>Meeting No</label>
                      <asp:TextBox runat="server" ID="txtMeetingNo" CssClass="form-control form-control-sm" />
                  </div>
              </div>
              <div class="col-md-3 filter-group filter-meeting">
                  <div class="form-group">
                      <label>Meeting Title</label>
                      <asp:TextBox runat="server" ID="txtTitle" CssClass="form-control form-control-sm" />
                  </div>
              </div>
              <div class="col-md-3 filter-group filter-meeting">
                  <div class="form-group">
                      <label>Meeting Category</label>
                      <asp:DropDownList runat="server" ID="ddlCategorySearch" CssClass="form-control form-control-sm" />
                  </div>
              </div>

              <div class="col-md-3 filter-group filter-meeting" style="display: none;">
                  <div class="form-group">
                      <label>Classification</label>
                      <asp:DropDownList runat="server" ID="ddlClassificationSearch" CssClass="form-control form-control-sm">
                          <asp:ListItem Value="">All</asp:ListItem>
                          <asp:ListItem Value="External">External</asp:ListItem>
                          <asp:ListItem Value="Internal">Internal</asp:ListItem>
                          <asp:ListItem Value="Others">Others</asp:ListItem>
                      </asp:DropDownList>
                  </div>
              </div>
              <div class="col-md-3 filter-group filter-meeting">
                  <div class="form-group">
                      <label>Meeting Held From Date</label>
                      <asp:TextBox runat="server" TextMode="Date" ID="txtMeetingDateFrom" CssClass="form-control form-control-sm" />
                  </div>
              </div>
              <div class="col-md-3 filter-group filter-meeting">
                  <div class="form-group">
                      <label>Meeting Held To Date</label>
                      <asp:TextBox runat="server" TextMode="Date" ID="txtMeetingDateTo" CssClass="form-control form-control-sm" />
                  </div>
              </div>
              <div class="col-md-3 created-by-fields">
                  <div class="form-group">
                      <label>Created By</label>
                      <asp:DropDownList runat="server" ID="ddlCreatedBy" CssClass="form-control form-control-sm" />
                  </div>
              </div>

              <div class="col-md-3 created-by-fields" style="display: none;">
                  <div class="form-group">
                      <label>Created Date From</label>
                      <asp:TextBox runat="server" TextMode="Date" ID="txtCreatedDate" CssClass="form-control form-control-sm" />
                  </div>
              </div>
              <div class="col-md-3 created-by-fields" style="display: none;">
                  <div class="form-group">
                      <label>Created Date To</label>
                      <asp:TextBox runat="server" TextMode="Date" ID="txtToDate" CssClass="form-control form-control-sm" />
                  </div>
              </div>
              <div class="col-md-3 filter-group filter-meeting" style="display: none;">
                  <div class="form-group">
                      <label>Meeting Status</label>
                      <asp:DropDownList runat="server" ID="ddlMeetingStatus" CssClass="form-control form-control-sm">
                          <asp:ListItem Value="">All</asp:ListItem>
                          <asp:ListItem Value="Upcoming">Upcoming</asp:ListItem>
                          <asp:ListItem Value="Today">Today</asp:ListItem>
                          <asp:ListItem Value="Completed">Completed</asp:ListItem>
                      </asp:DropDownList>
                  </div>
              </div>
              <div class="col-md-3 filter-group filter-approval" style="display: none;">
                  <div class="form-group">
                      <label>Approval Status</label>
                      <asp:DropDownList runat="server" ID="ddlApprovalStatus" CssClass="form-control form-control-sm">
                          <asp:ListItem Value="">All</asp:ListItem>
                          <asp:ListItem Value="Drafted">Drafted</asp:ListItem>
                          <asp:ListItem Value="Initiator">Pending</asp:ListItem>
                          <asp:ListItem Value="Verified">Ongoing</asp:ListItem>
                          <asp:ListItem Value="Approved">Approved</asp:ListItem>
                          <asp:ListItem Value="Returned">Returned</asp:ListItem>
                      </asp:DropDownList>
                  </div>
              </div>

              <div class="col-md-3 filter-group filter-agenda" style="display: none;">
                  <div class="form-group">
                      <label>Implementation Status</label>
                      <asp:DropDownList runat="server" ID="ddlImplementationStatus" CssClass="form-control form-control-sm">
                          <asp:ListItem Value="">All</asp:ListItem>
                          <asp:ListItem Value="Implemented">Implemented</asp:ListItem>
                          <asp:ListItem Value="Ongoing">Ongoing</asp:ListItem>
                          <asp:ListItem Value="Not Implemented">Not Implemented</asp:ListItem>
                      </asp:DropDownList>
                  </div>
              </div>
              <div class="col-md-3 filter-group filter-meeting">
                  <div class="form-group">
                      <label>Member / Employee ID</label>
                      <asp:ListBox runat="server" ID="ddlMemberEmployeeId" SelectionMode="Multiple" CssClass="form-control form-control-sm" />
                  </div>
              </div>
               <div class="col-md-8 offset-md-2 filter-group filter-minutes">
                   <div class="key-search-box">
                       <div class="key-search-header">
                           <label class="key-search-title">
                               <i class="fa fa-search mr-2"></i> Key Search
                           </label>
                           <span class="key-search-badge">
                               <i class="fa fa-magic mr-1"></i> Attachment Deep Scan
                           </span>
                       </div>
                       <div class="key-search-input-wrapper">
                           <i class="fa fa-keyboard-o search-icon"></i>
                           <asp:TextBox runat="server" ID="txtKeySearch" CssClass="form-control search-textbox"
                               placeholder="Search text inside PDF or image attachments..." />
                           <div class="file-types-hint">
                               <span class="badge-pdf"><i class="fa fa-file-pdf-o"></i> PDF</span>
                               <span class="badge-img"><i class="fa fa-file-image-o"></i> IMG</span>
                           </div>
                       </div>
                       <div class="key-search-footer-hint">
                           <i class="fa fa-info-circle text-info mr-1"></i>
                           Searches through OCR-extracted text and notes within uploaded meeting documents.
                       </div>
                   </div>
               </div>
          </div>

          <script type="text/javascript">
              function applyMeetingSearchSection() {
                  var selectedSection = $('#<%=rblSearchSection.ClientID%> input:checked').val() || 'All';
                  $('.meeting-search-panel .filter-group').hide();

                  if (selectedSection === 'Approval') {
                      $('.created-by-fields').hide();
                  } else {
                      $('.created-by-fields').show();
                  }

                  if (selectedSection === 'All') {
                      $('.meeting-search-panel .filter-group').show();
                  } else {
                      $('.meeting-search-panel .filter-' + selectedSection.toLowerCase()).show();
                  }

                  $('#<%=ddlClassificationSearch.ClientID%>, #<%=txtCreatedDate.ClientID%>, #<%=txtToDate.ClientID%>, #<%=ddlMeetingStatus.ClientID%>, #<%=ddlImplementationStatus.ClientID%>, #<%=ddlApprovalStatus.ClientID%>').closest('.col-md-3').hide();
              }

              function pageLoad() {
                  $('#<%=ddlCreatedBy.ClientID%>').chosen({ disable_search_threshold: 5, search_contains: true, width: '100%' });
                  $('#<%=ddlMemberEmployeeId.ClientID%>').chosen({ disable_search_threshold: 5, search_contains: true, width: '100%' });
                  applyMeetingSearchSection();
              }

              $(document).on('change', '#<%=rblSearchSection.ClientID%> input', applyMeetingSearchSection);
          </script>
       
       
            

                                    <div class="row">
                                        <div class="col-md-4">
                                            <%--<asp:Button runat="server" ID="Button51" OnClick="btn_Save_OnClick" Text="Search " CssClass="btn btn-outline-info btn-block disabled btn-sm" />--%>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:LinkButton runat="server" ID="btn_Search" OnClick="btn_Search_OnClick"   CssClass="btn btn-info   btn-sm"><span aria-hidden="true" class="fa fa-search-plus"></span>  &nbsp; Search </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="lbReset" OnClick="lbReset_OnClick" CssClass="btn btn-warning   btn-sm"><span aria-hidden="true" class="fa fa-retweet"></span>  &nbsp; Reset </asp:LinkButton>
                                        </div>
                                        <div class="col-md-4">
                                            <%--<asp:Button runat="server" ID="Button51" OnClick="btn_Save_OnClick" Text="Search " CssClass="btn btn-outline-info btn-block disabled btn-sm" />--%>
                                        </div>
                                    </div>
       
        <br />
       
                     <style>
        .pagination .page-item.active .page-link  {
            background-color: #007BFF !important;
            border-style: none !important;
            /* W3C */
        }
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.19/js/dataTables.bootstrap4.min.js"></script>
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/buttons/1.5.6/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.flash.min.js"></script>
    <script type="text/javascript" charset="utf8" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script type="text/javascript" charset="utf8" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
    <script type="text/javascript" charset="utf8" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.html5.min.js"></script>
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.print.min.js"></script>

      <script>

          $(document).ready(function () {
              // Setup - add a text input to each footer cell


              // DataTable
              var table = $('#cpFormBody_gv_ViewList').DataTable(
                   {
                       "bInfo": true,
                       "bFilter": true,
                       lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                       pageLength: 10,
                       dom: 'lBfrtip',

                       buttons: [


                       {
                           extend: 'excel',
                           footer: true,
                           text: '<i class="fa fa-file-excel-o" > Excel </i>',
                           titleAttr: 'Export to Excel'
                    ,
                           filename: 'Meeting Status List',
                           title: 'SMC',
                           messageTop: 'Meeting Status List',
                           exportOptions: {
                               columns: [0, 1, 2, 3, 4, 5]
                           }
                       }
                       ]
                   }
                  );

              var prm = Sys.WebForms.PageRequestManager.getInstance();
              if (prm != null) {
                  prm.add_endRequest(function (sender, e) {
                      if (sender._postBackSettings && sender._postBackSettings.panelsToUpdate != null) {
                          if ($.fn.DataTable.isDataTable('#cpFormBody_gv_ViewList')) {
                              $('#cpFormBody_gv_ViewList').DataTable().destroy();
                          }
                          table = $('#cpFormBody_gv_ViewList').DataTable(
                          {
                              "bInfo": true,
                              "bFilter": true,
                              lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                              pageLength: 10,
                              dom: 'lBfrtip',

                              buttons: [


                              {
                                  extend: 'excel',
                                  footer: true,
                                  text: '<i class="fa fa-file-excel-o" > Excel </i>',
                                  titleAttr: 'Export to Excel'
                           ,
                                  filename: 'Meeting Status List',
                                  title: 'SMC',
                                  messageTop: 'Meeting Status List',
                                  exportOptions: {
                                      columns: [0, 1, 2, 3, 4, 5]
                                  }
                              }
                              ]


                          }
                          );
                      }
                  });
              };

              // Apply the search
              table.columns().every(function () {
                  var that = this;

                  $('input', this.footer()).on('keyup change', function () {
                      if (that.search() !== this.value) {
                          that
                              .search(this.value)
                              .draw();
                      }
                  });
              });
          });

      </script>
    
       <style>
           .table   thead th {
               background-color: #5B799E;
               color: white;
               font-size: 13px !important;
               font-family: "Times New Roman", Times, serif !important;
               font-style: italic !important;
               font-weight: bold!important;
           }


          
                                              
                                              .dt-button.buttons-print,
                                               .dt-button.buttons-excel.buttons-html5,
                                               .dt-button.buttons-pdf.buttons-html5 {
                                                   background-color: white!important;
                                                    color:#880e4f !important;
                                                    border: none!important;
                                                   
                                                    padding: 5px 18px!important;
                                                   text-align: center!important;
                                                   text-decoration: none!important;
                                                   display: inline-block!important;
                                                   font-size: 16px!important;
                                                   margin: 2px 1px!important;
                                                   cursor: pointer!important;
                                                   -webkit-transition-duration: 0.4s!important;
                                                   transition-duration: 0.4s!important;
                                                   box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2), 0 3px 10px 0 rgba(0,0,0,0.19)!important;
                                               }


                                              .dt-buttons {
                                                  align-content: center;
                                                  text-align: right;
                                                  margin-top: -50px;
                                              }
                                              .dt-button.buttons-excel.buttons-html5:hover,
                                              .dt-button.buttons-pdf.buttons-html5:hover {
                                                  box-shadow: 0 12px 16px 0 rgba(0,0,0,0.24),0 17px 50px 0 rgba(0,0,0,0.19)!important;
                                                  color:white!important;
              background-color: #880e4f !important;
                                              }
                                         
       </style>

            <asp:GridView Width="100%" ShowHeader="True" ID="gv_ViewList" runat="server" AutoGenerateColumns="false" CssClass="AddToListCssTable"   OnPreRender="gv_DocumentUpload_PreRender">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL#">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>
                                                         
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Company">
                                                    <ItemTemplate>
                                                         
                                                        <asp:Label ID="lbl_ShortName"   runat="server" Text='<%#Eval("ShortName")%>'></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hfMeetingInfoID" Value='<%#Eval("MeetingInfoID")%>' />
                                                              <asp:HiddenField runat="server" ID="hfActionStatus" Value='<%#Eval("ActionStatus")%>' />

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                 
                                                <asp:TemplateField HeaderText="Category">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_MeetingCategory" runat="server" Text='<%#Eval("MeetingCategory") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                   <asp:TemplateField HeaderText="Title">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Title" runat="server" Text='<%#Eval("Title") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                              <asp:TemplateField HeaderText="Meeting Notes">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Purpose" runat="server" Text='<%#Eval("MeetingPurpose") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                     <asp:BoundField DataField="ActionStatusShow" HeaderText="Approval Status" Visible="False" />
                                        
                                        <asp:BoundField DataField="AwEmpName" HeaderText="Awaiting Employee" Visible="False" />
                                                   <asp:TemplateField HeaderText="Created By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CreateByName" runat="server" Text='<%#Eval("CreatedByEmployeeName") %>'></asp:Label>
                                                        <asp:PlaceHolder runat="server" Visible='<%# !string.IsNullOrWhiteSpace(Convert.ToString(Eval("CreatedByEmployeeId"))) %>'>
                                                            <br />
                                                            <asp:Label ID="lbl_CreateByEmployeeId" runat="server" Text='<%#Eval("CreatedByEmployeeId") %>'></asp:Label>
                                                        </asp:PlaceHolder>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                  <asp:BoundField DataField="CreateDate" HeaderText="Created Date" DataFormatString="{0:dd-MMM-yyyy}" Visible="False" /> 
                                                
                                                
                                                
<%--                                                  <asp:TemplateField HeaderText="Updated By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_UpdateBy" runat="server" Text='<%#Eval("UpdateBy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                  <asp:BoundField DataField="UpdateDate" HeaderText="Updated Date" DataFormatString="{0:dd-MMM-yyyy}" /> --%>


                                                <asp:TemplateField HeaderText="Actions">
                                                    <ItemTemplate>
                                                        
                                                        <asp:LinkButton runat="server" ID="btnEdit" OnClick="btnEdit_OnClick"   CssClass="btn btn-sm btn-warning"><i class="fa fa-pencil-square-o"></i> </asp:LinkButton>
                                                      <asp:LinkButton runat="server" ID="btnRemove" OnClick="btnRemove_OnClick"   CssClass="btn btn-sm btn-danger"><i class="fa fa-minus-circle"></i> </asp:LinkButton>
                                                        
                                                        <asp:LinkButton runat="server" ID="btnView" OnClick="btnView_OnClick"   CssClass="btn btn-sm btn-success"><i class="fa   fa-eye"></i> </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
     
           <br/> 
           <br/> 
           <br/> 
           <br/> 
           <br/> 
           <br/> 
           <br/> 
           <br/> 
           <br/> 
      
              </div>
           <br/> 
           <br/> 
           <br/> 
           <br/> 
           <br/> 
           <br/> 
           <br/> 
           <br/> 
        
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
                               </div>
        
        </div>
</asp:Content>

