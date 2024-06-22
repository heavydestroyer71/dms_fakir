
-- Create Procedure : SP_DOCUMENT_REPORT
-- Category Name: Report
-- Flowpath: Bill Status
   
-------------------------

insert into Sys_Category
(
CategoryName, Description, IsActive, EntryBy, EntryDate
) 
select 'Report' CategoryName, 'Report' Description, 1 IsActive,'1' EntryBy, getdate() EntryDate

--------------Then------------------

declare @CategoryId as int=(select max(CategoryId) from Sys_Category)

insert into Sys_Flowpath
(
CompanyId, CategoryId, FlowName, Description, TnaDays, IsRE, IsPO, IsPI, IsLC, IsMR, IsCL, IsBill, IsAmount, IsDiscount, IsAccounts, IsSupervisor, IsTeamMember, IsApprover, IsCloser, IsCanUpload, IsCanDownload, IsCanDelete, SerialNo, IsActive, EntryBy, EntryDate
) 
select 1 CompanyId, @CategoryId CategoryId, 'Bill Status' FlowName, 'Bill Status' as Description,0 TnaDays,0 IsRE,0 IsPO,0 IsPI,
0 IsLC, 0 IsMR, 0 IsCL, 0 IsBill, 0 IsAmount, 0 IsDiscount, 0 IsAccounts, 0 IsSupervisor,0 IsTeamMember,
0 IsApprover,0 IsCloser,0 IsCanUpload,0 IsCanDownload,0 IsCanDelete,99 SerialNo,
1 IsActive,'1' EntryBy, getdate() EntryDate

--------------Then-----------------
  
  -- Do Off Auto Increment on table Sys_Menu

  insert into Sys_Menu
  (
  ID, ParentId, Title, Description, Url, CategoryId, IsParentOnly, IsActive, SLNo
  )
  select 99 as ID, 0 as ParentId, 'Report' as Title, 'All Report' as Description, 
  null as Url, null as CategoryId, 0 as IsParentOnly, 1 as IsActive, 99.00 as SLNo

  -------------Then-------------------

  insert into Sys_Menu
  (
  ID, ParentId, Title, Description, Url, CategoryId, IsParentOnly, IsActive, SLNo
  )
  select 100 as ID, 99 as ParentId, 'Bill Status Report' as Title, 'All Document Report' as Description, 
  'UI/AllDocumentReport.aspx' as Url, 0 as CategoryId, 0 as IsParentOnly, 1 as IsActive, 1.00 as SLNo
  
  -------------------------------------------

  -- After That
  -- Do Menu Permision as Category: Report and Flow Path:Bill Status

  -- After That
  -- Do User wise Flowpath as Category: Report and Flow Name:Bill Status

  -- After That
  -- Do Publish 
