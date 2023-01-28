using FormsApp_OidcClient.Browser;
using IdentityModel.OidcClient;
using System.Text;

namespace FormsApp_OidcClient
{
    public partial class Form1 : Form
    {
        OidcClient _oidcClient;
        public Form1()
        {
            InitializeComponent();
            var options = new OidcClientOptions
            {
                //Authority = "https://demo.identityserver.io",
                //Authority = "http://keycloak.l2.ucnit.eu/auth/realms/TheGlassHouse/",
                //ClientId = "API",
                //ClientSecret = "2de46b9d-0afa-469f-b7a3-3d57c453888c",
                //Scope = "email profile roles",
                //RedirectUri = "https://localhost/winforms.client",
                Authority = System.Environment.GetEnvironmentVariable("AuthenticationServer"),
                ClientId = System.Environment.GetEnvironmentVariable("ClientId"),
                ClientSecret = System.Environment.GetEnvironmentVariable("ClientSecret"),
                Scope = System.Environment.GetEnvironmentVariable("Scope"),
                RedirectUri = System.Environment.GetEnvironmentVariable("RedirectURI"),
                Browser = new WinFormsWebView()
            };
            // IdentityModel.OidcClient will not accept self signed certificates
            options.Policy.Discovery.RequireHttps = false;
            _oidcClient = new OidcClient(options);
        }

        private async void BtLogin_Click(object sender, EventArgs e)
        {
            var result = await _oidcClient.LoginAsync();

            if (result.IsError)
            {
                MessageBox.Show(this, result.Error, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var sb = new StringBuilder(128);
                foreach (var claim in result.User.Claims)
                {
                    sb.AppendLine($"{claim.Type}: {claim.Value}");
                }

                if (!string.IsNullOrWhiteSpace(result.RefreshToken))
                {
                    sb.AppendLine();
                    sb.AppendLine($"refresh token: {result.RefreshToken}");
                }

                if (!string.IsNullOrWhiteSpace(result.IdentityToken))
                {
                    sb.AppendLine();
                    sb.AppendLine($"identity token: {result.IdentityToken}");
                }

                if (!string.IsNullOrWhiteSpace(result.AccessToken))
                {
                    sb.AppendLine();
                    sb.AppendLine($"access token: {result.AccessToken}");
                }

                TbResponse.Text = sb.ToString();
            }
        }
    }
}