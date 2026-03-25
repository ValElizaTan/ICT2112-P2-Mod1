using ProRental.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using ProRental.Domain.Enums;
using ProRental.Domain.Entities;
using ProRental.Controllers.Module1;
using ProRental.Data.Services;
using ProRental.Domain.Services;

// uncomment when ready to code
using ProRental.Data;
using ProRental.Domain.Controls;
using ProRental.Interfaces.Domain;
using ProRental.Interfaces.Data;
using ProRental.Controllers;
using ProRental.Domain.Module6.Controls;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var connectionString = builder.Configuration.GetConnectionString("Default");

// 2. Create the builder and map your strict PostgreSQL Enum
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.MapEnum<AccessEventType>("access_event_type", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<AlertStatus>("alert_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<AnalyticsType>("analytics_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<BatchStatus>("batch_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<CarbonStageType>("carbon_stage_type", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<CartStatus>("cart_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<CheckoutStatus>("checkout_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ClearanceBatchStatus>("clearance_batch_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ClearanceStatus>("clearance_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<DeliveryDuration>("delivery_duration_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<DeliveryType>("delivery_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<FileFormat>("file_format_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<HubType>("hub_type", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<InventoryStatus>("inventory_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<LoanStatus>("loan_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<LogType>("log_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<NotificationFrequency>("notification_frequency_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<NotificationGranularity>("notification_granularity_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<NotificationType>("notification_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<OrderHistoryStatus>("order_history_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<OrderStatus>("order_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<PaymentMethod>("payment_method_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<PaymentPurpose>("payment_purpose_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<POStatus>("po_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<PreferenceType>("preference_type", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ProductStatus>("product_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<PurchaseOrderStatus>("purchase_order_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<RatingBand>("rating_band_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<RentalStatus>("rental_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ReplenishmentReason>("reason_code_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ReplenishmentStatus>("replenishment_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ReturnItemStatus>("return_item_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ReturnRequestStatus>("return_request_status", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ReturnStatus>("return_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<ShipmentStatus>("shipment_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<StageType>("stagetype", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<SupplierCategory>("supplier_category_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<TransactionPurpose>("transaction_purpose_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<TransactionStatus>("transaction_status_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<TransactionType>("transaction_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<TransportMode>("transport_mode", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<UserRole>("user_role_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<VettingDecision>("vetting_decision_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<VettingResult>("vetting_result_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
dataSourceBuilder.MapEnum<VisualType>("visual_type_enum", new Npgsql.NameTranslation.NpgsqlNullNameTranslator());

// 3. Build the data source
var dataSource = dataSourceBuilder.Build();

// 4. Register the DbContext using the data source instead of a raw string
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql(dataSource));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dataSource, o =>
    {
        o.MapEnum<AccessEventType>("access_event_type");
        o.MapEnum<AlertStatus>("alert_status");
        o.MapEnum<AnalyticsType>("analytics_type_enum");
        o.MapEnum<BatchStatus>("batch_status");
        o.MapEnum<CarbonStageType>("carbon_stage_type");
        o.MapEnum<CartStatus>("cart_status_enum");
        o.MapEnum<CheckoutStatus>("checkout_status_enum");
        o.MapEnum<ClearanceBatchStatus>("clearance_batch_status");
        o.MapEnum<ClearanceStatus>("clearance_status");
        o.MapEnum<DeliveryDuration>("delivery_duration_enum");
        o.MapEnum<DeliveryType>("delivery_type_enum");
        o.MapEnum<FileFormat>("file_format_enum");
        o.MapEnum<HubType>("hub_type");
        o.MapEnum<InventoryStatus>("inventory_status");
        o.MapEnum<LoanStatus>("loan_status");
        o.MapEnum<LogType>("log_type_enum");
        o.MapEnum<NotificationFrequency>("notification_frequency_enum");
        o.MapEnum<NotificationGranularity>("notification_granularity_enum");
        o.MapEnum<NotificationType>("notification_type_enum");
        o.MapEnum<OrderHistoryStatus>("order_history_status_enum");
        o.MapEnum<OrderStatus>("order_status_enum");
        o.MapEnum<PaymentMethod>("payment_method_enum");
        o.MapEnum<PaymentPurpose>("payment_purpose_enum");
        o.MapEnum<POStatus>("po_status_enum");
        o.MapEnum<PreferenceType>("preference_type");
        o.MapEnum<ProductStatus>("product_status");
        o.MapEnum<PurchaseOrderStatus>("purchase_order_status_enum");
        o.MapEnum<RatingBand>("rating_band_enum");
        o.MapEnum<RentalStatus>("rental_status_enum");
        o.MapEnum<ReplenishmentReason>("reason_code_enum");
        o.MapEnum<ReplenishmentStatus>("replenishment_status_enum");
        o.MapEnum<ReturnItemStatus>("return_item_status");
        o.MapEnum<ReturnRequestStatus>("return_request_status");
        o.MapEnum<ReturnStatus>("return_status_enum");
        o.MapEnum<ShipmentStatus>("shipment_status_enum");
        o.MapEnum<StageType>("stagetype");
        o.MapEnum<SupplierCategory>("supplier_category_enum");
        o.MapEnum<TransactionPurpose>("transaction_purpose_enum");
        o.MapEnum<TransactionStatus>("transaction_status_enum");
        o.MapEnum<TransactionType>("transaction_type_enum");
        o.MapEnum<TransportMode>("transport_mode");
        o.MapEnum<UserRole>("user_role_enum");
        o.MapEnum<VettingDecision>("vetting_decision_enum");
        o.MapEnum<VettingResult>("vetting_result_enum");
        o.MapEnum<VisualType>("visual_type_enum");
    }));

//Services builder(add your mappers/gateways, controllers, control and interface classes here)
//Team P2-1
// Data source

// Domain

// Presentation/Controllers


//Team P2-2
// Data source

// Domain

// Presentation/Controllers

//Team P2-3
// Data source

// Domain

// Presentation/Controllers


//Team P2-4
// Data source
builder.Services.AddScoped<ProRental.Data.Module1.Interfaces.ICustomerGateway, ProRental.Data.Module1.Gateways.CustomerGateway>();
builder.Services.AddScoped<ProRental.Data.Module1.Interfaces.IStaffGateway, ProRental.Data.Module1.Gateways.StaffGateway>();
builder.Services.AddScoped<ProRental.Data.Module1.Interfaces.INotificationGateway, ProRental.Data.Module1.Gateways.NotificationGateway>();
builder.Services.AddScoped<ProRental.Data.Module1.Interfaces.INotificationPreferenceGateway, ProRental.Data.Module1.Gateways.NotificationPreferenceGateway>();

// Domain
builder.Services.AddScoped<ProRental.Domain.Module1.P24.Interfaces.ICustomerService, ProRental.Domain.Module1.P24.Controls.CustomerControl>();
builder.Services.AddScoped<ProRental.Domain.Module1.P24.Interfaces.IStaffService, ProRental.Domain.Module1.P24.Controls.StaffControl>();
builder.Services.AddScoped<ProRental.Domain.Module1.P24.Controls.StaffControl>();
builder.Services.AddScoped<ProRental.Domain.Module1.P24.Controls.CustomerControl>();
builder.Services.AddScoped<ProRental.Domain.Module1.P24.Interfaces.INotificationPreferenceService, ProRental.Domain.Module1.P24.Controls.NotificationPreferenceControl>();
builder.Services.AddScoped<ProRental.Domain.Module1.P24.Controls.NotificationManager>();
builder.Services.AddScoped<ProRental.Domain.Module1.P24.Interfaces.INotificationSubject>(provider => provider.GetRequiredService<ProRental.Domain.Module1.P24.Controls.NotificationManager>());

// Presentation/Controllers


//Team P2-5
// Data source

// Domain

// Presentation/Controllers


//Team P2-6
// Data source
builder.Services.AddScoped<ICatalogueService, CatalogueService>();
builder.Services.AddScoped<IOrderMapper, OrderMapper>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IShippingOptionService, FakeShippingService>();
builder.Services.AddScoped<ISessionMapper, SessionMapper>();
builder.Services.AddScoped<IAuthenticationService, ProRentalAuthenticationService>();
builder.Services.AddScoped<ICustomerValidationService, CustomerValidationService>();
builder.Services.AddScoped<ICartMapper, ProRental.Data.Module1.Gateways.CartMapper>();
builder.Services.AddScoped<ICheckoutMapper, ProRental.Data.Module1.Gateways.CheckoutMapper>();
builder.Services.AddSingleton<IPaymentProviderClient, MockPaymentProviderClient>();
builder.Services.AddScoped<IPaymentAdaptors, StripeAdapter>();
builder.Services.AddScoped<IPaymentAdaptors, PayPalAdapter>();
builder.Services.AddScoped<IPaymentAdaptors, AdyenAdapter>();
 
// Domain (controls — pure business logic, no DB dependency)
builder.Services.AddScoped<IPaymentAdaptorSelector, PaymentAdaptorSelector>();
builder.Services.AddScoped<IPaymentGatewayService, PaymentGatewayControl>();

// Domain
builder.Services.AddScoped<CatalogueControl>();
builder.Services.AddScoped<IOrderService, OrderManagementControl>();
builder.Services.AddScoped<ISessionService, SessionControl>();
builder.Services.AddScoped<AuthenticationControl>();
builder.Services.AddScoped<CustomerIDValidationControl>();
builder.Services.AddScoped<ICartService, CartControl>();
builder.Services.AddScoped<CartSessionControl>();
builder.Services.AddScoped<CartItemControl>();
builder.Services.AddScoped<CartSelectionControl>();
builder.Services.AddScoped<CartQueryControl>();
builder.Services.AddScoped<CartCheckoutControl>();
builder.Services.AddScoped<ICheckoutService, CheckoutControl>();
builder.Services.AddScoped<CheckoutLifecycleControl>();
builder.Services.AddScoped<CheckoutShippingControl>();
builder.Services.AddScoped<CheckoutPaymentControl>();
builder.Services.AddScoped<ICostCalculation, CostCalculationControl>();
builder.Services.AddScoped<CheckoutCostControl>();
builder.Services.AddScoped<OrderBuilderControl>();
builder.Services.AddScoped<CheckoutNotificationControl>();

// Auth
builder.Services.AddScoped<IAuthenticationService, ProRentalAuthenticationService>();
builder.Services.AddScoped<ICustomerValidationService, CustomerValidationService>();

// Presentation/Controllers
builder.Services.AddScoped<CatalogueController>();
builder.Services.AddScoped<Module1Controller>();


builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddHttpContextAccessor();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();      
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
