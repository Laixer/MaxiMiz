using System;

namespace Maximiz.Mapper
{

    /// <summary>
    /// 
    /// </summary>
    internal static partial class MapperEnum
    {

        internal static Model.Enums.BidStrategy Map(ViewModels.Enums.BidStrategy bidStrategy)
        {
            switch (bidStrategy)
            {
                case ViewModels.Enums.BidStrategy.Smart:
                    return Model.Enums.BidStrategy.Smart;
                case ViewModels.Enums.BidStrategy.Fixed:
                    return Model.Enums.BidStrategy.Fixed;
            }

            throw new InvalidOperationException(nameof(bidStrategy));
        }

        internal static Model.Enums.BudgetModel Map(ViewModels.Enums.BudgetModel budgetModel)
        {
            switch (budgetModel)
            {
                case ViewModels.Enums.BudgetModel.Campaign:
                    return Model.Enums.BudgetModel.Campaign;
                case ViewModels.Enums.BudgetModel.Monthly:
                    return Model.Enums.BudgetModel.Monthly;
            }

            throw new InvalidOperationException(nameof(budgetModel));
        }

        internal static Model.Enums.Delivery Map(ViewModels.Enums.Delivery delivery)
        {
            switch (delivery)
            {
                case ViewModels.Enums.Delivery.Balanced:
                    return Model.Enums.Delivery.Balanced;
                case ViewModels.Enums.Delivery.Accelerated:
                    return Model.Enums.Delivery.Accelerated;
                case ViewModels.Enums.Delivery.Strict:
                    return Model.Enums.Delivery.Strict;
            }

            throw new InvalidOperationException(nameof(delivery));
        }

        internal static Model.Enums.Device Map(ViewModels.Enums.Device device)
        {
            switch (device)
            {
                case ViewModels.Enums.Device.Mobile:
                    return Model.Enums.Device.Mobile;
                case ViewModels.Enums.Device.Tablet:
                    return Model.Enums.Device.Tablet;
                case ViewModels.Enums.Device.Laptop:
                    return Model.Enums.Device.Laptop;
                case ViewModels.Enums.Device.Desktop:
                    return Model.Enums.Device.Desktop;
                case ViewModels.Enums.Device.Wearable:
                    return Model.Enums.Device.Wearable;
            }

            throw new InvalidOperationException(nameof(device));
        }

        internal static Model.Enums.OS Map(ViewModels.Enums.OS os)
        {
            switch (os)
            {
                case ViewModels.Enums.OS.Windows:
                    return Model.Enums.OS.Windows;
                case ViewModels.Enums.OS.Linux:
                    return Model.Enums.OS.Linux;
                case ViewModels.Enums.OS.OSX:
                    return Model.Enums.OS.OSX;
                case ViewModels.Enums.OS.Android:
                    return Model.Enums.OS.Android;
                case ViewModels.Enums.OS.iOS:
                    return Model.Enums.OS.IOS;
                case ViewModels.Enums.OS.Unix:
                    return Model.Enums.OS.Unix;
                case ViewModels.Enums.OS.Chromeos:
                    return Model.Enums.OS.Chromeos;
            }

            throw new InvalidOperationException(nameof(os));
        }

        internal static Model.Enums.Location Map(ViewModels.Enums.Location location)
        {
            switch (location)
            {
                case ViewModels.Enums.Location.NL:
                    return Model.Enums.Location.NL;
                case ViewModels.Enums.Location.UK:
                    return Model.Enums.Location.UK;
                case ViewModels.Enums.Location.ES:
                    return Model.Enums.Location.ES;
                case ViewModels.Enums.Location.DE:
                    return Model.Enums.Location.DE;
                case ViewModels.Enums.Location.FR:
                    return Model.Enums.Location.FR;
            }

            throw new InvalidOperationException(nameof(location));
        }

        internal static ViewModels.Enums.Location Map(Model.Enums.Location location)
        {
            switch (location)
            {
                case Model.Enums.Location.NL:
                    return ViewModels.Enums.Location.NL;
                case Model.Enums.Location.UK:
                    return ViewModels.Enums.Location.UK;
                case Model.Enums.Location.ES:
                    return ViewModels.Enums.Location.ES;
                case Model.Enums.Location.DE:
                    return ViewModels.Enums.Location.DE;
                case Model.Enums.Location.FR:
                    return ViewModels.Enums.Location.FR;
            }

            throw new InvalidOperationException(nameof(location));
        }

        internal static Model.Enums.Publisher Map(ViewModels.Enums.Publisher publisher)
        {
            switch (publisher)
            {
                case ViewModels.Enums.Publisher.Unknown:
                    return Model.Enums.Publisher.Unknown;
                case ViewModels.Enums.Publisher.Google:
                    return Model.Enums.Publisher.Google;
                case ViewModels.Enums.Publisher.Taboola:
                    return Model.Enums.Publisher.Taboola;
                case ViewModels.Enums.Publisher.Outbrain:
                    return Model.Enums.Publisher.Outbrain;
                case ViewModels.Enums.Publisher.Adroll:
                    return Model.Enums.Publisher.Adroll;
                case ViewModels.Enums.Publisher.Criteo:
                    return Model.Enums.Publisher.Criteo;
            }

            throw new InvalidOperationException(nameof(publisher));
        }

    }
}
