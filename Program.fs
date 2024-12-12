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
