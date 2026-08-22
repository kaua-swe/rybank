using src.DTOs.Ticket;
using src.Interfaces.Account;
using src.Interfaces.Auth;
using src.Interfaces.Bank;
using src.Interfaces.Pix;
using src.Interfaces.Register;
using src.Interfaces.Ticket;

namespace src.Services.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;
        private readonly IBankService _bankService;
        private readonly IPixService _pixService;
        private readonly ITicketService _ticketService;

        public RegisterService(IAuthService authService, IAccountService accountService, IBankService bankService, IPixService pixService,
        ITicketService ticketService)
        {
            _authService = authService;
            _accountService = accountService;
            _bankService = bankService;
            _pixService = pixService;
            _ticketService = ticketService;
        }

        public async Task<object> CreateUser(string email, string senha, string nomeCompleto, string cpf, string telefone, string tipoConta)
        {
            var userRegister = await _authService.CreateUser(email, senha);

            var userRegisterAccount = await _accountService.CreateAccount(email, nomeCompleto, cpf, telefone);

            var userRegisterWallet = await _bankService.CreateWallet(email, tipoConta);

            return new
            {
                userRegister,
                userRegisterAccount,
                userRegisterWallet
            };
        }

        public async Task<object> UpdateUser(string email, string nomeCompleto, string cpf, string telefone)
        {

            var userUpdateAccount = await _accountService.UpdateAccount(email, nomeCompleto, cpf, telefone);

            return new
            {
                userUpdateAccount
            };
        }

        public async Task<object> Authentication(string email, string senha)
        {
            var userLogin = await _authService.Authentication(email, senha);

            return new
            {
                userLogin
            };
        }

        public async Task<object> CreateWallet(string email, string tipoConta)
        {
            var createWallet = await _bankService.CreateWallet(email, tipoConta);

            return new
            {
                createWallet
            };
        }

        public async Task<object> Deposit(string email, decimal valor, string tipoConta)
        {
            var newDeposit = await _bankService.Deposit(email, valor, tipoConta);

            return new
            {
                newDeposit
            };
        }

        public async Task<object> Sacar(string email, string tipoConta, decimal valor)
        {
            
            var newSaque = await _bankService.Sacar(email, tipoConta, valor);

            return new
            {
                newSaque
            };
        }

        public async Task<object> Transfer(string email, string tipo, string chave, string conta, decimal valor)
        {
            var newTrasnfer = await _bankService.Transfer(email, tipo, chave, conta, valor);

            return new
            {
                newTrasnfer
            };
        }

        public async Task<object> RegisterKeyPix(string email, string tipoChave, string chave, string conta)
        {
            var newKey =  await _pixService.RegisterKeyPix(email, tipoChave, chave, conta);

            return new
            {
                newKey
            };
        }

        public async Task<object> DeleteKey(string email, string chave)
        {
            var deleteKey = await _pixService.DeleteKey(email, chave);

            return new
            {
                deleteKey
            };
        }

        public async Task<object> CreateTicket(string empresa, string cnpj, int vencimento, decimal valor)
        {
            
            var newTicket = await _ticketService.CreateTicket(empresa, cnpj, vencimento, valor);

            return new
            {
                newTicket
            };
        }

        public async Task<object> CancellTicket(string codigo)
        {
            
            var cancellTicket = await _ticketService.CancellTicket(codigo);

            return new
            {
                cancellTicket
            };
        }

        public async Task<List<ResponseTicketDto>> ListTicket(string empresa)
        {
            var listTicket = await _ticketService.ListTicket(empresa);

            return listTicket;
        }

        public async Task<object> PayTicket(string email, string codigo, string conta)
        {
            
            var payTicket = await _ticketService.PayTicket(email, codigo, conta);

            return new
            {
                payTicket
            };
        }
    }
}