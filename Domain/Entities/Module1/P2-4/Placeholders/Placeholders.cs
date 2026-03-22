namespace Domain.Entities.Module1.P2_4.Placeholders
{
    // PLACEHOLDER — owned by Module3Team1
    public interface IBatchDelivery { }

    // PLACEHOLDER — owned by Module3Team1
    public interface IShippingOption { }

    // PLACEHOLDER — owned by Module1Team6
    public interface IOrderService { }

    // PLACEHOLDER — owner TBC, not owned by Shipping module
    // This is the class causing your error
    public class DeliveryMethod { }

    // PLACEHOLDER — owned by Module1Team4 (internal, different feature)
    public class RefundControl { }

    // PLACEHOLDER — owned by Module1Team4 (internal, different feature)
    public class StaffDashboardControl { }

    // PLACEHOLDER — owned by Module1Team4 (internal, different feature)
    public class WalkInOrderControl { }
}
