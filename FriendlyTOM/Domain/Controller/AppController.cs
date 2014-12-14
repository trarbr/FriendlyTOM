using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Controller
{
    public class AppController
    {

        public static AppController Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new AppController();
                }

                return _instance; 
            }
        }
        private static AppController _instance;

        public CustomerController CustomerController
        {
            get { return _customerController; }

        }
        public SupplierController SupplierController
        {
            get { return _supplierController; }
        }
        public PaymentController PaymentController
        {
            get { return _paymentController; }
        }
        public BookingController BookingController
        {
            get { return _bookingController; }
        }
        public SettingsController SettingsController
        {
            get { return _settingsController; }
        }
        private CustomerController _customerController;
        private SupplierController _supplierController;
        private PaymentController _paymentController;
        private BookingController _bookingController;
        private SettingsController _settingsController;

        private AppController()
        {
        }

        public void Initialize(DataAccess.IDataAccessFacade dataAccessFacade = null)
        {
            try
            {
                if (dataAccessFacade == null)
                {
                    dataAccessFacade = DataAccess.DataAccessFacade.Instance;
                }

                _settingsController = new SettingsController(dataAccessFacade);
                _settingsController.FirstRunSetup();

                _customerController = new CustomerController(dataAccessFacade);
                _supplierController = new SupplierController(dataAccessFacade);
                _bookingController = new BookingController(dataAccessFacade);
                _paymentController = new PaymentController(dataAccessFacade);

                // NOTE: The order of initialization is important! Reordering it can break stuff!
                _customerController.Initialize(_bookingController, _paymentController);
                _supplierController.Initialize(_bookingController, _paymentController);
                _bookingController.Initialize(_customerController, _paymentController);
                _paymentController.Initialize();
            }
            catch
            {
                throw;
            }
        }
    }
}
