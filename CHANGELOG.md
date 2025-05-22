# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

### Added
- SME and SV toggle switches with automatic value mapping
- Debug logging for key press detection (`Debug.WriteLine`)
- Enter key support for submitting customer entry

### Fixed
- Customer folder now renames correctly on name update
- Prevented runtime errors by cleaning orphaned databases

### Changed
- Deleting a customer now also deletes their NAS folder
- Added error handling for folder deletion issues

### Added
- Edit/Update customer functionality
  - Pre-fills input fields
  - Updates database record
  - Renames NAS folder if customer name changed

### Added
- Search/filter feature for customer list:
  - Real-time filtering by name or email
  - Automatically updates the DataGrid as user types


### Added
- Delete Customer feature:
  - Shows confirmation dialog before deleting
  - Removes selected record from the database
  - Refreshes the customer list automatically


### Added
- View Customer List feature using Dapper
  - `DataGrid` UI to display all customers
  - Method to load customers from `customer.db`
  - Automatically loads list on startup and after adding a customer
  - Refresh button to reload the customer list manually


### Added
- Initial project structure with WinUI 3 and .NET 8
- Login system with role-based access (`admin`, `user`)
- Customer entry form with SQLite database per user
- Auto-folder creation for each customer in NAS
- Git version control initialized with GitHub repository
- SSH authentication setup with GitHub

### Fixed
- SQLite warning about missing WinAppRuntime
- Enter key now triggers “Add Customer” button

## [2025-05-21]
- First stable version of the Customer Manager app
