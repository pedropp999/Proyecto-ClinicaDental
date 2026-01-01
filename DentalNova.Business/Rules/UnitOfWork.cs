using DentalNova.Core.Interfaces;
using DentalNova.Core.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Business.Rules
{
    public class UnitOfWork : IUnitOfWork
    {
        public ITratamientoBL Tratamiento { get; }
        public IUsuarioBL Usuario { get; }
        public IArticuloBL Articulo { get; }
        public ICompraArticuloBL CompraArticulo { get; }
        public IRecordatorioBL Recordatorio { get; }
        public IPacienteBL Paciente { get; }
        public ICitaBL Cita { get; }
        public IOdontologoBL Odontologo { get; }
        public IHorarioOdontologoBL HorarioOdontologo { get; }

        public UnitOfWork(
            ITratamientoBL tratamientoBL, 
            IUsuarioBL usuarioBL,
            IArticuloBL articuloBL,
            ICompraArticuloBL compraArticuloBL,
            IRecordatorioBL recordatorioBL,
            IPacienteBL pacienteBL,
            ICitaBL citaBL,
            IOdontologoBL odontologoBL,
            IHorarioOdontologoBL horarioOdontologoBL
            )
        {
            Tratamiento = tratamientoBL;
            Usuario = usuarioBL;
            Articulo = articuloBL;
            CompraArticulo = compraArticuloBL;
            Recordatorio = recordatorioBL;
            Paciente = pacienteBL;
            Cita = citaBL;
            Odontologo = odontologoBL;
            HorarioOdontologo = horarioOdontologoBL;
        }
    }
}
