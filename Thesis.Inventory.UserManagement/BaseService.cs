using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.Inventory.UserManagement
{
    using AutoMapper;
    using Thesis.Inventory.Authentication.Configurations;
    using Thesis.Inventory.Infrastructure.UnitOfWork;

    /// <summary>
    /// The user management base service.
    /// </summary>
    public class BaseService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        /// <param name="thesisUnitOfWork">IUserUnit of work parameter..</param>
        public BaseService(IThesisUnitOfWork thesisUnitOfWork, IMapper mapper)
        {
            this.ThesisUnitOfWork = thesisUnitOfWork;
            this.Mapper = mapper;
        }

        public BaseService(IThesisUnitOfWork thesisUnitOfWork, IMapper mapper, JwtSettings jwtSettings)
        {
            this.ThesisUnitOfWork = thesisUnitOfWork;
            this.Mapper = mapper;
            this.JwtSettings = jwtSettings;
        }

        /// <summary>
        /// Gets or sets the SQLUnitOfWork.
        /// </summary>
        protected internal IThesisUnitOfWork ThesisUnitOfWork { get; set; }

        /// <summary>
        /// Gets or sets the Mapper.
        /// </summary>
        protected internal IMapper Mapper { get; set; }

        /// <summary>
        /// Gets or sets the Mapper.
        /// </summary>
        protected internal JwtSettings JwtSettings { get; set; }
    }
}
