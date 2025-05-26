# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

### UI/UX Improvements
- Added signed-in user info in NavigationView with rounded initials avatar
- Centered avatar and name vertically for a modern Fluent layout
- Removed separator line, added subtle spacing for cleaner hierarchy

### Added
- UI/UX: Add signed-in user block in NavigationView with initials avatar and polished spacing
- Display user initials in a rounded avatar above the name
- Stack initials and name vertically, centered in NavView
- Added clean vertical spacing between user block and menu items (no separator line)
- Improved visual hierarchy and alignment for better Fluent-style navigation experience


### Added
- Adaptive NavigationView layout with automatic pane switching (Auto mode).
- Custom Fluent-style title bar using NavigationView.Header.
- Animated icon buttons for Add, Refresh, Open Folder, and Delete.

### Changed
- Replaced MainWindow with Shell window following Fluent design.
- Restructured app navigation to use NavigationView.
- Improved dark theme appearance and visual hierarchy.
- Resized default window to 900x600 for better startup UX.
- Moved title "Customer Manager" into NavigationView header for consistent styling.

### Fixed
- Runtime crashes due to incorrect `Background` on `<Window>`.
- Errors caused by broken XAML tag closure.
- COMException from XAML designer when resources were misapplied.
- Double title header issue on `CustomerPage`.

### Added
- Automatically create Photos and Videos subfolders for each customer.

### Added
- Open Folder button per customer row for quick access

### Fixed
- Resolved XAML error with multiple elements in DataGridTemplateColumn

### Changed
- Removed global Delete button and added per-row delete icon
- Converted customer edit to use popup ContentDialog
- Improved layout and spacing for minimalist UI
- Added double-click edit trigger on DataGrid row

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
- Fix: Prevent empty Name/Email in Add Customer dialog
- Fix: Inline validation message prevents nested dialog runtime error
- Fix: Logout icon display and async method error resolved
- Refactor: Moved logout logic from SelectionChanged to ItemInvoked for better clarity
