using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Orderstatushistory
{
    private int _historyid;
    private int Historyid { get => _historyid; set => _historyid = value; }

    private int _orderitemid;
    private int Orderitemid { get => _orderitemid; set => _orderitemid = value; }

    private DateTime _timestamp;
    private DateTime Timestamp { get => _timestamp; set => _timestamp = value; }

    private string _updatedby = null!;
    private string Updatedby { get => _updatedby; set => _updatedby = value; }

    private string? _remark;
    private string? Remark { get => _remark; set => _remark = value; }
}
