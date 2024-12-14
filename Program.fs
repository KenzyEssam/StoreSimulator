type Product = { Name: string; Price: decimal; Description: string }

let productCatalog = [
    { Name = "Laptop"; Price = 1000m; Description = "High performance laptop" }
    { Name = "Smartphone"; Price = 500m; Description = "Latest model smartphone" }
    { Name = "Headphones"; Price = 100m; Description = "Noise-cancelling headphones" }
    { Name = "Keyboard"; Price = 50m; Description = "Mechanical keyboard" }
    { Name = "Mouse"; Price = 30m; Description = "Wireless mouse" }
    { Name = "Tablet"; Price = 400m; Description = "Lightweight and powerful tablet" }
    { Name = "Smartwatch"; Price = 200m; Description = "Feature-rich smartwatch" }
    { Name = "Monitor"; Price = 300m; Description = "4K Ultra HD monitor" }
    { Name = "Speaker"; Price = 150m; Description = "Bluetooth portable speaker" }
    { Name = "Camera"; Price = 800m; Description = "DSLR camera with advanced features" }
    { Name = "Gaming Console"; Price = 500m; Description = "Next-gen gaming console" }
    { Name = "Drone"; Price = 700m; Description = "High-resolution camera drone" }
    { Name = "Router"; Price = 100m; Description = "High-speed Wi-Fi router" }
    { Name = "External Hard Drive"; Price = 120m; Description = "1TB portable hard drive" }
    { Name = "E-Reader"; Price = 150m; Description = "Compact and glare-free e-reader" }

]


let cart = ref [] 

let form = new Form(Text = "Store Simulator", Width = 800, Height = 600, BackColor = Color.FromArgb(242, 242, 242)) 
form.StartPosition <- FormStartPosition.CenterScreen
form.Font <- new Font("Segoe UI", 10.0f)

let catalogPanel = new Panel(Dock = DockStyle.Fill, BackColor = Color.White)
let catalogListBox = new ListBox(Width = 500, Height = 350, Font = new Font("Segoe UI", 14.0f), ForeColor = Color.FromArgb(51, 51, 51), BackColor = Color.White, BorderStyle = BorderStyle.None)
catalogListBox.SelectionMode <- SelectionMode.One
catalogListBox.ItemHeight <- 80

for product in productCatalog do
    catalogListBox.Items.Add($"{product.Name} - ${product.Price}: {product.Description}") |> ignore

let addToCartButton = new Button(Text = "Add to Cart", Width = 250, Height = 50, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold))
addToCartButton.Top <- 380
addToCartButton.Left <- 20
addToCartButton.FlatStyle <- FlatStyle.Flat
addToCartButton.FlatAppearance.BorderSize <- 0
addToCartButton.MouseEnter.Add(fun _ -> addToCartButton.BackColor <- Color.FromArgb(0, 105, 217)) 
addToCartButton.MouseLeave.Add(fun _ -> addToCartButton.BackColor <- Color.FromArgb(0, 123, 255))

let viewCartButton = new Button(Text = "View Cart", Width = 250, Height = 50, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold))
viewCartButton.Top <- 450
viewCartButton.Left <- 20
viewCartButton.FlatStyle <- FlatStyle.Flat
viewCartButton.FlatAppearance.BorderSize <- 0
viewCartButton.MouseEnter.Add(fun _ -> viewCartButton.BackColor <- Color.FromArgb(0, 105, 217)) 
viewCartButton.MouseLeave.Add(fun _ -> viewCartButton.BackColor <- Color.FromArgb(0, 123, 255))

catalogPanel.Controls.Add(catalogListBox)
catalogPanel.Controls.Add(addToCartButton)
catalogPanel.Controls.Add(viewCartButton)

let cartPanel = new Panel(Dock = DockStyle.Fill, BackColor = Color.White)
let cartListBox = new ListBox(Width = 300, Height = 350, Font = new Font("Segoe UI", 10.0f), ForeColor = Color.FromArgb(51, 51, 51), BackColor = Color.White, BorderStyle = BorderStyle.None)
cartListBox.SelectionMode <- SelectionMode.One
cartListBox.ItemHeight <- 40

let removeFromCartButton = new Button(Text = "Remove from Cart", Width = 250, Height = 50, BackColor = Color.FromArgb(255, 0, 0), ForeColor = Color.White, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold))
removeFromCartButton.Top <- 380
removeFromCartButton.Left <- 20
removeFromCartButton.FlatStyle <- FlatStyle.Flat
removeFromCartButton.FlatAppearance.BorderSize <- 0
removeFromCartButton.MouseEnter.Add(fun _ -> removeFromCartButton.BackColor <- Color.FromArgb(217, 0, 0)) 
removeFromCartButton.MouseLeave.Add(fun _ -> removeFromCartButton.BackColor <- Color.FromArgb(255, 0, 0))

let totalLabel = new Label(Text = "Total: $0.00", Font = new Font("Segoe UI", 14.0f), Top = 450, Left = 20, Width = 530, ForeColor = Color.FromArgb(51, 51, 51), TextAlign = ContentAlignment.MiddleLeft)

let checkoutButton = new Button(Text = "Checkout", Width = 250, Height = 50, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold))
checkoutButton.Top <- 510
checkoutButton.Left <- 20
checkoutButton.FlatStyle <- FlatStyle.Flat
checkoutButton.FlatAppearance.BorderSize <- 0
checkoutButton.MouseEnter.Add(fun _ -> checkoutButton.BackColor <- Color.FromArgb(0, 105, 217)) 
checkoutButton.MouseLeave.Add(fun _ -> checkoutButton.BackColor <- Color.FromArgb(0, 123, 255))

<<<<<<< HEAD
backButton.Click.Add(fun _ ->
    form.Controls.Clear()
    form.Controls.Add(catalogPanel)
    catalogPanel.Visible <- true
    form.BackgroundImage <- null 
    catalogPanel.Visible <- true
=======
cartPanel.Controls.Add(cartListBox)
cartPanel.Controls.Add(removeFromCartButton)
cartPanel.Controls.Add(totalLabel)
cartPanel.Controls.Add(checkoutButton)

let backButton = new Button(Text = "Back", Width = 100, Height = 40, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 10.0f))
backButton.Top <- 20
backButton.Left <- 660
backButton.FlatStyle <- FlatStyle.Flat
backButton.FlatAppearance.BorderSize <- 0
backButton.MouseEnter.Add(fun  -> backButton.BackColor <- Color.FromArgb(0, 105, 217)) 
backButton.MouseLeave.Add(fun  -> backButton.BackColor <- Color.FromArgb(0, 123, 255))
cartPanel.Controls.Add(backButton)


try
    form.BackgroundImage <- Image.FromFile(@"shopbackground.jpg") 
    form.BackgroundImageLayout <- ImageLayout.Stretch
with
| ex -> MessageBox.Show("Error loading image: " + ex.Message) |> ignore

let shopNowButton = new Button(Text = "Shop Now", Width = 200, Height = 60, BackColor = Color.FromArgb(0, 0, 139), ForeColor = Color.White, Font = new Font("Segoe UI", 14.0f, FontStyle.Bold))
shopNowButton.Left <- (form.ClientSize.Width - shopNowButton.Width) / 2
shopNowButton.Top <- (form.ClientSize.Height - shopNowButton.Height) / 2 + 240
shopNowButton.FlatStyle <- FlatStyle.Flat
shopNowButton.FlatAppearance.BorderSize <- 0
form.Controls.Add(shopNowButton)


shopNowButton.Click.Add(fun  -> 
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
>>>>>>> 791fe327c2f86516c4528b2f7630cac4866201a3
    cartPanel.Visible <- true
)