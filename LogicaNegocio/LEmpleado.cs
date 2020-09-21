using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Utilitarios;

namespace LogicaNegocio
{
    public class LEmpleado
    {
        //METODO PARA OBTENER PRODUCTOS EN PEDIDOS ATENDER
        public List<EncapProducto_pedido> ObtenerProductos( int pedido)
        { 
            var productos = new DAOEmpleado().ObtenerProductos(pedido);
            return productos;
        }
        //METODO PARA OBTENER PEDIDOS POR EMPLEADO
        public List<UEncapPedido> ObtenerPedidos(int user)
        {
            var pedidos = new DAOEmpleado().ObtenerPedidos(user);
            return pedidos;
        }
        //METODO PARA ACTUALIZAR EL ESTADO DEL PEDIDO A 2
        public void ActualizarEstadoPedido2(UEncapPedido pedido2)
        {
            new DAOEmpleado().ActualizarEstadoPedido2(pedido2);
        }
        //METODO PARA ACTUALIZAR LA NOVEDAD DEL PEDIDO
        public void ActualizarNovedadPedido(UEncapPedido novedad)
        {
            new DAOEmpleado().ActualizarNovedadPedido(novedad);
        }

        //METODO PARA ACTUALIZAR EL ESTADO DEL PEDIDO A 3
        public void ActualizarEstadoPedido3(UEncapPedido pedido3)
        {
            new DAOEmpleado().ActualizarEstadoPedido3(pedido3);
        }
        //METODO PARA ACTUALIZAR ESTADO DEL EMPLEADO
        public void ActualizarEstadoEmpleado(UEncapUsuario empleado)
        {
            new DAOEmpleado().ActualizarEstadoEmpleado(empleado);
        }
    }
}
