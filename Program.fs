open System
open System.Drawing
open System.Windows.Forms
open System.IO
open System.Text.Json

type Product = { Name: string; Price: decimal; Description: string }

let loadProducts filePath =
    try
        let json = File.ReadAllText(filePath)
        JsonSerializer.Deserialize<Product list>(json)
    with
    | ex -> 
        MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        []

let productCatalog = loadProducts @"Products.json"

let cart = ref [] // Using a reference to hold the list for mutability

// Create the main form
let form = new Form(Text = "Store Simulator", Width = 800, Height = 600, BackColor = Color.FromArgb(242, 242, 242)) 
form.StartPosition <- FormStartPosition.CenterScreen
form.Font <- new Font("Segoe UI", 10.0f)

// Initial Welcome Screen
try
    form.BackgroundImage <- Image.FromFile(@"shop_background.jpg") 
    form.BackgroundImageLayout <- ImageLayout.Stretch
with
| ex -> MessageBox.Show("Error loading image: " + ex.Message) |> ignore

let shopNowButton = new Button(Text = "Shop Now", Width = 200, Height = 60, BackColor = Color.FromArgb(0, 0, 139), ForeColor = Color.White, Font = new Font("Segoe UI", 14.0f, FontStyle.Bold))
shopNowButton.Left <- (form.ClientSize.Width - shopNowButton.Width) / 2
shopNowButton.Top <- (form.ClientSize.Height - shopNowButton.Height) / 2 + 240
shopNowButton.FlatStyle <- FlatStyle.Flat
shopNowButton.FlatAppearance.BorderSize <- 0
form.Controls.Add(shopNowButton)

// Catalog Panel
let catalogPanel = new Panel(Dock = DockStyle.Fill, BackColor = Color.FromArgb(211, 211, 211)) 
let catalogListBox = new ListBox(
    Width = 783, 
    Height = 420, 
    Font = new Font("Segoe UI", 16.0f), 
    ForeColor = Color.FromArgb(51, 51, 51), 
    BackColor =  Color.FromArgb(173, 216, 230), 
    BorderStyle = BorderStyle.FixedSingle)
catalogListBox.SelectionMode <- SelectionMode.One
catalogListBox.ItemHeight <- 100 
catalogListBox.DrawMode <- DrawMode.OwnerDrawVariable

for product in productCatalog do
    catalogListBox.Items.Add($"{product.Name} - ${product.Price}: {product.Description}") |> ignore


catalogListBox.DrawItem.Add(fun e ->
    if e.Index >= 0 then
        let itemText = catalogListBox.Items.[e.Index].ToString()
        
        let productNameEndIndex = itemText.IndexOf(" -")
        let productName = itemText.Substring(0, productNameEndIndex)
        let restOfText = itemText.Substring(productNameEndIndex)

        let boldFont = new Font("Segoe UI", 16.0f, FontStyle.Bold)
        let regularFont = new Font("Segoe UI", 16.0f, FontStyle.Regular)

        let baseColor =
            if isHovered then Color.FromArgb(0, 120, 215) 
            else if isFocused then Color.FromArgb(173, 216, 230) 
            else Color.FromArgb(173, 216, 230) 

        let textColor = Color.White 

        use bgBrush = new SolidBrush(baseColor)
        e.Graphics.FillRectangle(bgBrush, e.Bounds)

        let productNameRect = new Rectangle(e.Bounds.X + 10, e.Bounds.Y + 10, e.Bounds.Width - 20, e.Bounds.Height)
        TextRenderer.DrawText(e.Graphics, productName, boldFont, productNameRect, textColor, TextFormatFlags.Left)

        let restTextRect = new Rectangle(e.Bounds.X + 10, e.Bounds.Y + 40, e.Bounds.Width - 20, e.Bounds.Height)
        TextRenderer.DrawText(e.Graphics, restOfText, regularFont, restTextRect, textColor, TextFormatFlags.Left)

        if isFocused then
            ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, textColor, baseColor)

)

let addToCartButton = new Button(Text = "Add to Cart", Width = 250, Height = 70, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold))
addToCartButton.Top <- 450
addToCartButton.Left <- 70
addToCartButton.FlatStyle <- FlatStyle.Standard
addToCartButton.FlatAppearance.BorderSize <- 0
addToCartButton.MouseEnter.Add(fun _ -> addToCartButton.BackColor <- Color.FromArgb(0, 0, 139)) 
addToCartButton.MouseLeave.Add(fun _ -> addToCartButton.BackColor <- Color.FromArgb(0, 123, 255))

let viewCartButton = new Button(Text = "View Cart", Width = 250, Height = 70, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold))
viewCartButton.Top <- 450
viewCartButton.Left <- addToCartButton.Left + addToCartButton.Width + 140
viewCartButton.FlatStyle <- FlatStyle.Standard
viewCartButton.FlatAppearance.BorderSize <- 0
viewCartButton.MouseEnter.Add(fun _ -> viewCartButton.BackColor <- Color.FromArgb(0, 0, 139)) 
viewCartButton.MouseLeave.Add(fun _ -> viewCartButton.BackColor <- Color.FromArgb(0, 123, 255))

catalogPanel.Controls.Add(catalogListBox)
catalogPanel.Controls.Add(addToCartButton)
catalogPanel.Controls.Add(viewCartButton)

// Cart Panel
let cartPanel = new Panel(Dock = DockStyle.Fill, BackColor = Color.FromArgb(211, 211, 211))
let cartListBox = new ListBox(Width = 600, Height = 350, Font = new Font("Segoe UI", 10.0f), ForeColor = Color.FromArgb(51, 51, 51), BackColor = Color.FromArgb(211, 211, 211) , BorderStyle = BorderStyle.FixedSingle)
cartListBox.SelectionMode <- SelectionMode.One
cartListBox.ItemHeight <- 40
cartListBox.DrawMode <- DrawMode.OwnerDrawVariable

cartListBox.DrawItem.Add(fun e -> 
    if e.Index >= 0 then
        // Get the item text
        let itemText = cartListBox.Items.[e.Index].ToString()

        // Split the text into product name and price
        let productNameEndIndex = itemText.IndexOf(" -")
        let productName = itemText.Substring(0, productNameEndIndex)
        let priceText = itemText.Substring(productNameEndIndex)

        // Define fonts
        let boldFont = new Font("Segoe UI", 16.0f, FontStyle.Bold)
        let regularFont = new Font("Segoe UI", 16.0f, FontStyle.Regular)

        // Determine the state of the current item
        let isHovered = (e.State &&& DrawItemState.Selected) = DrawItemState.Selected
        let isFocused = (e.State &&& DrawItemState.Focus) = DrawItemState.Focus

        // Define background colors
        let baseColor =
            if isHovered then Color.FromArgb(0, 120, 215)  // Lighter blue for hover
            else if isFocused then Color.FromArgb(173, 216, 230) // Light blue for focus
            else Color.FromArgb(173, 216, 230) // Default light blue

        let textColor = Color.White // White text for contrast on dark background

        // Fill the background for the current item
        use bgBrush = new SolidBrush(baseColor)
        e.Graphics.FillRectangle(bgBrush, e.Bounds)

        // Define space for product name and price on the same line
        let nameWidth = e.Bounds.Width * 2 / 3 // Product name takes up 2/3 of the width
        let priceWidth = e.Bounds.Width - nameWidth // Price takes up the remaining space

        // Draw the product name in bold
        let productNameRect = new Rectangle(e.Bounds.X + 10, e.Bounds.Y + 10, nameWidth - 20, e.Bounds.Height)
        TextRenderer.DrawText(e.Graphics, productName, boldFont, productNameRect, textColor, TextFormatFlags.Left)

        // Draw the price in regular font
        let priceTextRect = new Rectangle(e.Bounds.X + nameWidth, e.Bounds.Y + 10, priceWidth - 20, e.Bounds.Height)
        TextRenderer.DrawText(e.Graphics, priceText, regularFont, priceTextRect, textColor, TextFormatFlags.Left)

        // Draw focus rectangle if the item has focus
        if isFocused then
            ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, textColor, baseColor)
)

let removeFromCartButton = new Button(Text = "Remove", Width = 200, Height = 50, BackColor = Color.FromArgb(0, 0, 139), ForeColor = Color.White, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold))
removeFromCartButton.Top <- 380
removeFromCartButton.Left <- 20
removeFromCartButton.FlatStyle <- FlatStyle.Standard
removeFromCartButton.FlatAppearance.BorderSize <- 0
removeFromCartButton.MouseEnter.Add(fun _ -> removeFromCartButton.BackColor <- Color.FromArgb(217, 0, 0)) 
removeFromCartButton.MouseLeave.Add(fun _ -> removeFromCartButton.BackColor <- Color.FromArgb(0, 0, 139))

let totalLabel = new Label(Text = "Total: $0.00", Font = new Font("Segoe UI", 14.0f), Top = 450, Left = 20, Width = 530, ForeColor = Color.FromArgb(51, 51, 51), TextAlign = ContentAlignment.MiddleLeft)

let checkoutButton = new Button(Text = "Checkout", Width = 200, Height = 50, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold))
checkoutButton.Top <- 380 // Same line as Remove button
checkoutButton.Left <- removeFromCartButton.Left + removeFromCartButton.Width + 180 
checkoutButton.FlatStyle <- FlatStyle.Standard
checkoutButton.FlatAppearance.BorderSize <- 0
checkoutButton.MouseEnter.Add(fun _ -> checkoutButton.BackColor <- Color.FromArgb(144, 238, 144)) 
checkoutButton.MouseLeave.Add(fun _ -> checkoutButton.BackColor <- Color.FromArgb(0, 123, 255))

let backButton = new Button(Text = "Back", Width = 100, Height = 40, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 10.0f))
backButton.Top <- 20
backButton.Left <- 660
backButton.FlatStyle <- FlatStyle.Standard
backButton.FlatAppearance.BorderSize <- 0
backButton.MouseEnter.Add(fun _ -> backButton.BackColor <- Color.FromArgb(173, 216, 230)) 
backButton.MouseLeave.Add(fun _ -> backButton.BackColor <- Color.FromArgb(0, 123, 255))

let makeRoundedButton (button: Button) =
    let roundedRect = new Drawing2D.GraphicsPath()
    roundedRect.AddArc(0.0f, 0.0f, 20.0f, 20.0f, 180.0f, 90.0f) // Top-left corner
    roundedRect.AddArc(float32 (button.Width - 20), 0.0f, 20.0f, 20.0f, 270.0f, 90.0f) // Top-right corner
    roundedRect.AddArc(float32 (button.Width - 20), float32 (button.Height - 20), 20.0f, 20.0f, 0.0f, 90.0f) // Bottom-right corner
    roundedRect.AddArc(0.0f, float32 (button.Height - 20), 20.0f, 20.0f, 90.0f, 90.0f) // Bottom-left corner
    roundedRect.CloseFigure()
    button.Region <- new Region(roundedRect)

makeRoundedButton addToCartButton
makeRoundedButton viewCartButton
makeRoundedButton removeFromCartButton
makeRoundedButton checkoutButton
makeRoundedButton backButton
makeRoundedButton shopNowButton

cartPanel.Controls.Add(cartListBox)
cartPanel.Controls.Add(removeFromCartButton)
cartPanel.Controls.Add(totalLabel)
cartPanel.Controls.Add(checkoutButton)
cartPanel.Controls.Add(backButton)


// Functionality
shopNowButton.Click.Add(fun _ -> 
    form.Controls.Clear()
    form.Controls.Add(catalogPanel)
    catalogPanel.Visible <- true
)

viewCartButton.Click.Add(fun _ ->
    cartListBox.Items.Clear()
    for product in !cart do
        cartListBox.Items.Add($"{product.Name} - ${product.Price}")  |> ignore
    form.Controls.Clear()
    form.Controls.Add(cartPanel)
    cartPanel.Visible <- true
)

backButton.Click.Add(fun _ ->
    form.Controls.Clear()
    form.Controls.Add(catalogPanel)
    catalogPanel.Visible <- true
    form.BackgroundImage <- null // Disable background image
    catalogPanel.Visible <- true
    cartPanel.Visible <- true
)

addToCartButton.Click.Add(fun _ -> 
    if catalogListBox.SelectedItem <> null then
        let selectedItem = catalogListBox.SelectedItem.ToString().Split('-').[0].Trim()
        match productCatalog |> List.tryFind (fun p -> p.Name = selectedItem) with
        | Some product ->
            cart := product :: !cart
            MessageBox.Show($"{product.Name} has been added to the cart.", "Added", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
        | None -> MessageBox.Show("Error adding product to the cart.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
    else MessageBox.Show("Please select a product first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
)


removeFromCartButton.Click.Add(fun _ ->
    if cartListBox.SelectedItem <> null then
        let selectedItem = cartListBox.SelectedItem.ToString().Split('-').[0].Trim()
        match !cart |> List.tryFind (fun p -> p.Name = selectedItem) with
        | Some product ->
            match !cart |> List.tryFindIndex (fun p -> p.Name = selectedItem) with
            | Some index ->
                cart := List.mapi (fun i p -> if i = index then None else Some p) !cart |> List.choose id
                cartListBox.Items.Remove(cartListBox.SelectedItem)
                MessageBox.Show($"{product.Name} has been removed from the cart.", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
            | None -> MessageBox.Show("Error removing product from the cart.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        | None -> MessageBox.Show("Error removing product from the cart.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
    else MessageBox.Show("Please select a product to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
)

[<STAThread>]
Application.Run(form)