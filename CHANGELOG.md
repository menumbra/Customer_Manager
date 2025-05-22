# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

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
