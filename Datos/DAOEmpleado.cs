using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Datos
{
    public class DAOEmpleado
    {
        //METODO PARA OBTENER LOS PRODUCTOS DEL PEDIDO
        public List<EncapProducto_pedido> ObtenerProductos(int pedido)
        {
            using (var db = new Mapeo())

                return (from produc in db.productos.Where(x => x.Pedido_id == pedido)
                        join inventario in db.inventario on produc.Producto_id equals inventario.Id

                        select new
                        {
                            produc,
                            inventario


                        }).ToList().Select(m => new EncapProducto_pedido
                        {

                            Id = m.produc.Id,
                            Pedido_id = m.produc.Pedido_id,
                            Producto_id = m.produc.Producto_id,
                            Cantidad = m.produc.Cantidad,
                            Precio = m.produc.Precio,
                            Total = m.produc.Total,
                            Nombre_producto = m.inventario.Titulo,
                            Referencia = m.inventario.Referencia


                        }).ToList();
        }

        //METODO PARA OBTENER LOS PEDIDOS DEL EMPLEADO
        public List<UEncapPedido> ObtenerPedidos(int user)
        {
            using (var db = new Mapeo())

                return (from pedi in db.pedidos.Where(x => x.Atendido_id == user && x.Estado_pedido == 2)
                        join usu in db.usuario on pedi.User_id equals usu.User_id
                        //join emple in db.usuario  on pedi.Atendido_id  equals emple.User_id


                        select new
                        {
                            pedi,
                            usu
                            //emple,


                        }).ToList().Select(m => new UEncapPedido
                        {

                            Id = m.pedi.Id,
                            Fecha_pedido = m.pedi.Fecha_pedido,
                            User_id = m.pedi.User_id,
                            Atendido_id = m.pedi.Atendido_id,
                            Domiciliario_id = m.pedi.Domiciliario_id,
                            Estado_pedido = m.pedi.Estado_pedido,
                            Total = m.pedi.Total,
                            Novedad = m.pedi.Novedad,
                            Ciu_dep_id = m.pedi.Ciu_dep_id,
                            Direccion = m.pedi.Direccion,
                            Municipio_id = m.pedi.Municipio_id,
                            Fecha_pedido_fin = m.pedi.Fecha_pedido_fin,
                            Usuario = m.usu.Nombre
                            //Empleado = m.emple.Nombre,               


                        }).ToList();
        }

        //ACTUALIZAR ESTADO PEDIDO A 2
        public void ActualizarEstadoPedido2(UEncapPedido pedido2)
        {
            using (var db = new Mapeo())
            {
                UEncapPedido estado = db.pedidos.Where(x => x.Id == pedido2.Id).SingleOrDefault();
                estado.Estado_pedido = pedido2.Estado_pedido;
                db.SaveChanges();
            }
        }

        //ACTUALIZAR NOVEDAD EN EL PEDIDO
        public void ActualizarNovedadPedido(UEncapPedido novedad)
        {
            using (var db = new Mapeo())
            {
                UEncapPedido newnovedad = db.pedidos.Where(x => x.Id == novedad.Id).SingleOrDefault();
                newnovedad.Novedad = novedad.Novedad;

                db.SaveChanges();
            }
        }
        //ACTUALIZAR ESTADO PEDIDO A 3
        public void ActualizarEstadoPedido3(UEncapPedido pedido3)
        {
            using (var db = new Mapeo())
            {
                UEncapPedido estado = db.pedidos.Where(x => x.Id == pedido3.Id).SingleOrDefault();
                estado.Estado_pedido = pedido3.Estado_pedido;
                db.SaveChanges();
            }
        }
        //ACTUALIZAR ESTADO EMPLEADO
        public void ActualizarEstadoEmpleado(UEncapUsuario empleado)
        {
            using (var db = new Mapeo())
            {
                UEncapUsuario emple = db.usuario.Where(x => x.User_id == empleado.User_id).SingleOrDefault();
                emple.Estado_id = empleado.Estado_id;

                db.SaveChanges();
            }
        }

    }
}
