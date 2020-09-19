﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Datos
{
    public class DAOEmpleado
    {
        //METODO PARA VEIRIFICAR EL USUARIO ACTIVO
        public UEncapUsuario UsuarioActivo(string session)
        {
            using (var db = new Mapeo())
            {
                return db.usuario.Where(x => x.Sesion == session).FirstOrDefault();
            }
        }
        //METODO PARA VERIFICAR SI EXISTE REGISTRADO EL CORREO
        public UEncapUsuario verificarCorreo(UEncapUsuario verificar)
        {
            using (var db = new Mapeo())
            {
                return db.usuario.Where(x => x.Correo.Equals(verificar.Correo)).FirstOrDefault();
            }
        }
        //METODO PARA ACTUALIZAR EL USUARIO
        public void ActualizarUsuario(UEncapUsuario user)
        {
            using (var db = new Mapeo())
            {

                var resultado = db.usuario.SingleOrDefault(b => b.User_id == user.User_id);
                if (resultado != null)
                {
                    resultado.Nombre = user.Nombre;
                    resultado.Apellido = user.Apellido;
                    resultado.Correo = user.Correo;
                    resultado.Clave = user.Clave;
                    resultado.Fecha_nacimiento = user.Fecha_nacimiento;
                    resultado.Identificacion = user.Identificacion;
                    resultado.Token = user.Token;
                    resultado.Estado_id = user.Estado_id;
                    resultado.Rol_id = user.Rol_id;
                    resultado.Tiempo_token = user.Tiempo_token;
                    resultado.Sesion = user.Sesion;
                    resultado.Last_modify = DateTime.Now;
                    resultado.Ip_ = user.Ip_;
                    resultado.Mac_ = user.Mac_;

                    db.SaveChanges();
                }

            }

        }
        //OBTENGO CANTIDAD ACTUAL DEL INVENTARIO Y LE RESTO LA CANTIDAD QUE SE ENCUENTRA EN EL CARRITO
        public int ObtenerCantidadxProductoCarrito(int producto_id)
        {
            using (var db = new Mapeo())
            {
                //return db.inventario.Where(x => x.Id == producto_id).Select(x=> x.Ca_actual).First() - db.carrito.Where(x => x.Prod ucto_id == producto_id).Sum(x => x.Cantidad);
                Nullable<int> carrito = db.carrito.Where(x => x.Producto_id == producto_id).Sum(x => x.Cantidad);
                int cantidad = db.inventario.Where(x => x.Id == producto_id).Select(x => x.Ca_actual).First();
                return cantidad - (carrito != null ? carrito.Value : 0);
            }
        }
        //METODO PARA VERIFICAR SI EL USUARIO TIENE PEDIDO
        public UEncapCarrito verificarUsuarioTienePedido(UEncapCarrito verificar)
        {
            using (var db = new Mapeo())
            {
                return db.carrito.Where(x => x.User_id.Equals(verificar.User_id) && x.Estadocar.Equals(2)).FirstOrDefault();
            }
        }
        //METODO PARA VERIFICAR SI EL ITEM SELECCIONADO YA ESTA EN LA LISTA 
        public UEncapCarrito verificarArticuloEnCarrito(UEncapCarrito verificar)
        {
            using (var db = new Mapeo())
            {
                return db.carrito.Where(x => x.Producto_id.Equals(verificar.Producto_id) && x.User_id.Equals(verificar.User_id)).FirstOrDefault();
            }
        }
        //METODO ACTUALIZAR + ITEMS EN CARRITO 
        public void ActualizarCarritoItems(UEncapCarrito carrito)
        {
            using (var db = new Mapeo())
            {
                var resultado = db.carrito.FirstOrDefault(x => x.Producto_id == carrito.Producto_id && x.User_id == carrito.User_id);
                if (resultado != null)
                {
                    resultado.Id_Carrito = resultado.Id_Carrito;
                    resultado.User_id = resultado.User_id;
                    resultado.Producto_id = resultado.Producto_id;
                    resultado.Cantidad = resultado.Cantidad + carrito.Cantidad;
                    resultado.Fecha = resultado.Fecha;
                    resultado.Precio = resultado.Precio;
                    resultado.Total = resultado.Total + (carrito.Cantidad * resultado.Precio).Value;
                    resultado.Session = resultado.Session;
                    resultado.Last_modify = DateTime.Now;
                    db.SaveChanges();
                }
            }

        }
        //METODO PARA INSERTAR carrrito
        public void InsertarCarrito(UEncapCarrito carrito)
        {
            using (var db = new Mapeo())
            {
                db.carrito.Add(carrito);
                db.SaveChanges();
            }
        }
        public List<UEncapInventario> ConsultarInventario()
        {
            using (var db = new Mapeo())
            {
                return (from uu in db.inventario.Where(x => x.Ca_actual > 0)
                        join marca_carro in db.marca_carro on uu.Id_marca equals marca_carro.Id
                        join categoria in db.categoria on uu.Id_categoria equals categoria.Id
                        join estadoitem in db.estado_item on uu.Id_estado equals estadoitem.Id
                        let _cantCarrito = (from ss in db.carrito where ss.Producto_id == uu.Id select ss.Cantidad).Sum()

                        select new
                        {
                            uu,
                            marca_carro,
                            categoria,
                            estadoitem,
                            _cantCarrito


                        }).ToList().Select(m => new UEncapInventario
                        {
                            Id = m.uu.Id,
                            Imagen = m.uu.Imagen,
                            Titulo = m.uu.Titulo,
                            Precio = m.uu.Precio,
                            Referencia = m.uu.Referencia,
                            Ca_actual = m.uu.Ca_actual - (m._cantCarrito.HasValue ? m._cantCarrito.Value : 0),
                            Ca_minima = m.uu.Ca_minima,
                            Id_marca = m.uu.Id_marca,
                            Id_categoria = m.uu.Id_categoria,
                            Id_estado = m.uu.Id_estado,
                            Nombre_categoria = m.categoria.Categoria,
                            Nombre_marca = m.marca_carro.Marca,
                            Estado = m.estadoitem.Estado_item

                        }).ToList();
            }
        }
        //METODO CONSULTAR INVENTARIO CATEGORIA MENOS LA CANTIDAD DEL CARRITO 
        public List<UEncapInventario> ConsultarInventarioCategoria(int categ)
        {
            using (var db = new Mapeo())
            {
                return (from uu in db.inventario.Where(x => x.Ca_actual > 0 && x.Id_categoria == categ)
                        join marca_carro in db.marca_carro on uu.Id_marca equals marca_carro.Id
                        join categoria in db.categoria on uu.Id_categoria equals categoria.Id
                        join estadoitem in db.estado_item on uu.Id_estado equals estadoitem.Id
                        let _cantCarrito = (from ss in db.carrito where ss.Producto_id == uu.Id select ss.Cantidad).Sum()

                        select new
                        {
                            uu,
                            marca_carro,
                            categoria,
                            estadoitem,
                            _cantCarrito


                        }).ToList().Select(m => new UEncapInventario
                        {
                            Id = m.uu.Id,
                            Imagen = m.uu.Imagen,
                            Titulo = m.uu.Titulo,
                            Precio = m.uu.Precio,
                            Referencia = m.uu.Referencia,
                            Ca_actual = m.uu.Ca_actual - (m._cantCarrito.HasValue ? m._cantCarrito.Value : 0),
                            Ca_minima = m.uu.Ca_minima,
                            Id_marca = m.uu.Id_marca,
                            Id_categoria = m.uu.Id_categoria,
                            Id_estado = m.uu.Id_estado,
                            Nombre_categoria = m.categoria.Categoria,
                            Nombre_marca = m.marca_carro.Marca,
                            Estado = m.estadoitem.Estado_item

                        }).ToList();
            }
        }
        public List<UEncapInventario> ConsultarInventariMarca(int marca)
        {
            using (var db = new Mapeo())
            {
                return (from uu in db.inventario.Where(x => x.Ca_actual > 0 && x.Id_marca == marca)
                        join marca_carro in db.marca_carro on uu.Id_marca equals marca_carro.Id
                        join categoria in db.categoria on uu.Id_categoria equals categoria.Id
                        join estadoitem in db.estado_item on uu.Id_estado equals estadoitem.Id
                        let _cantCarrito = (from ss in db.carrito where ss.Producto_id == uu.Id select ss.Cantidad).Sum()

                        select new
                        {
                            uu,
                            marca_carro,
                            categoria,
                            estadoitem,
                            _cantCarrito


                        }).ToList().Select(m => new UEncapInventario
                        {
                            Id = m.uu.Id,
                            Imagen = m.uu.Imagen,
                            Titulo = m.uu.Titulo,
                            Precio = m.uu.Precio,
                            Referencia = m.uu.Referencia,
                            Ca_actual = m.uu.Ca_actual - (m._cantCarrito.HasValue ? m._cantCarrito.Value : 0),
                            Ca_minima = m.uu.Ca_minima,
                            Id_marca = m.uu.Id_marca,
                            Id_categoria = m.uu.Id_categoria,
                            Id_estado = m.uu.Id_estado,
                            Nombre_categoria = m.categoria.Categoria,
                            Nombre_marca = m.marca_carro.Marca,
                            Estado = m.estadoitem.Estado_item

                        }).ToList();
            }
        }
        public List<UEncapInventario> ConsultarInventariCombinado(int marca, int categ)
        {
            using (var db = new Mapeo())
            {
                return (from uu in db.inventario.Where(x => x.Ca_actual > 0 && x.Id_marca == marca && x.Id_categoria == categ)
                        join marca_carro in db.marca_carro on uu.Id_marca equals marca_carro.Id
                        join categoria in db.categoria on uu.Id_categoria equals categoria.Id
                        join estadoitem in db.estado_item on uu.Id_estado equals estadoitem.Id
                        let _cantCarrito = (from ss in db.carrito where ss.Producto_id == uu.Id select ss.Cantidad).Sum()

                        select new
                        {
                            uu,
                            marca_carro,
                            categoria,
                            estadoitem,
                            _cantCarrito


                        }).ToList().Select(m => new UEncapInventario
                        {
                            Id = m.uu.Id,
                            Imagen = m.uu.Imagen,
                            Titulo = m.uu.Titulo,
                            Precio = m.uu.Precio,
                            Referencia = m.uu.Referencia,
                            Ca_actual = m.uu.Ca_actual - (m._cantCarrito.HasValue ? m._cantCarrito.Value : 0),
                            Ca_minima = m.uu.Ca_minima,
                            Id_marca = m.uu.Id_marca,
                            Id_categoria = m.uu.Id_categoria,
                            Id_estado = m.uu.Id_estado,
                            Nombre_categoria = m.categoria.Categoria,
                            Nombre_marca = m.marca_carro.Marca,
                            Estado = m.estadoitem.Estado_item

                        }).ToList();
            }
        }
        public List<UEncapInventario> ConsultarInventarioPrecio(string valores)
        {
            //Metodo de Slip
            string[] split = valores.Split(',');
            int can1 = Convert.ToInt32(split[0]);
            int can2 = Convert.ToInt32(split[1]);


            using (var db = new Mapeo())
            {
                return (from uu in db.inventario.Where(x => x.Precio >= can1 && x.Precio <= can2)
                        join marca_carro in db.marca_carro on uu.Id_marca equals marca_carro.Id
                        join categoria in db.categoria on uu.Id_categoria equals categoria.Id
                        join estadoitem in db.estado_item on uu.Id_estado equals estadoitem.Id

                        let _cantCarrito = (from ss in db.carrito where ss.Producto_id == uu.Id select ss.Cantidad).Sum()

                        select new
                        {
                            uu,
                            marca_carro,
                            categoria,
                            estadoitem,

                            _cantCarrito

                        }).ToList().Select(m => new UEncapInventario

                        {
                            Id = m.uu.Id,
                            Imagen = m.uu.Imagen,
                            Titulo = m.uu.Titulo,
                            Precio = m.uu.Precio,
                            Referencia = m.uu.Referencia,
                            Ca_actual = m.uu.Ca_actual - (m._cantCarrito.HasValue ? m._cantCarrito.Value : 0),
                            Ca_minima = m.uu.Ca_minima,
                            Id_marca = m.uu.Id_marca,
                            Id_categoria = m.uu.Id_categoria,

                            Id_estado = m.uu.Id_estado,

                            Nombre_categoria = m.categoria.Categoria,
                            Nombre_marca = m.marca_carro.Marca,

                            Estado = m.estadoitem.Estado_item





                        }


                        ).ToList();

            }
        }
        public List<UEncapInventario> ConsultarInventarioPrecioCategoria(string valores, int categ)
        {
            //Metodo de Slip
            string[] split = valores.Split(',');
            int can1 = Convert.ToInt32(split[0]);
            int can2 = Convert.ToInt32(split[1]);


            using (var db = new Mapeo())
            {
                return (from uu in db.inventario.Where(x => x.Precio >= can1 && x.Precio <= can2 && x.Id_categoria == categ)
                        join marca_carro in db.marca_carro on uu.Id_marca equals marca_carro.Id
                        join categoria in db.categoria on uu.Id_categoria equals categoria.Id
                        join estadoitem in db.estado_item on uu.Id_estado equals estadoitem.Id

                        let _cantCarrito = (from ss in db.carrito where ss.Producto_id == uu.Id select ss.Cantidad).Sum()

                        select new
                        {
                            uu,
                            marca_carro,
                            categoria,
                            estadoitem,

                            _cantCarrito

                        }).ToList().Select(m => new UEncapInventario

                        {
                            Id = m.uu.Id,
                            Imagen = m.uu.Imagen,
                            Titulo = m.uu.Titulo,
                            Precio = m.uu.Precio,
                            Referencia = m.uu.Referencia,
                            Ca_actual = m.uu.Ca_actual - (m._cantCarrito.HasValue ? m._cantCarrito.Value : 0),
                            Ca_minima = m.uu.Ca_minima,
                            Id_marca = m.uu.Id_marca,
                            Id_categoria = m.uu.Id_categoria,

                            Id_estado = m.uu.Id_estado,

                            Nombre_categoria = m.categoria.Categoria,
                            Nombre_marca = m.marca_carro.Marca,

                            Estado = m.estadoitem.Estado_item





                        }


                        ).ToList();

            }
        }
        public List<UEncapInventario> ConsultarInventarioPrecioMarca(string valores, int marca)
        {
            //Metodo de Slip
            string[] split = valores.Split(',');
            int can1 = Convert.ToInt32(split[0]);
            int can2 = Convert.ToInt32(split[1]);


            using (var db = new Mapeo())
            {
                return (from uu in db.inventario.Where(x => x.Precio >= can1 && x.Precio <= can2 && x.Id_marca == marca)
                        join marca_carro in db.marca_carro on uu.Id_marca equals marca_carro.Id
                        join categoria in db.categoria on uu.Id_categoria equals categoria.Id
                        join estadoitem in db.estado_item on uu.Id_estado equals estadoitem.Id

                        let _cantCarrito = (from ss in db.carrito where ss.Producto_id == uu.Id select ss.Cantidad).Sum()

                        select new
                        {
                            uu,
                            marca_carro,
                            categoria,
                            estadoitem,

                            _cantCarrito

                        }).ToList().Select(m => new UEncapInventario

                        {
                            Id = m.uu.Id,
                            Imagen = m.uu.Imagen,
                            Titulo = m.uu.Titulo,
                            Precio = m.uu.Precio,
                            Referencia = m.uu.Referencia,
                            Ca_actual = m.uu.Ca_actual - (m._cantCarrito.HasValue ? m._cantCarrito.Value : 0),
                            Ca_minima = m.uu.Ca_minima,
                            Id_marca = m.uu.Id_marca,
                            Id_categoria = m.uu.Id_categoria,

                            Id_estado = m.uu.Id_estado,

                            Nombre_categoria = m.categoria.Categoria,
                            Nombre_marca = m.marca_carro.Marca,

                            Estado = m.estadoitem.Estado_item





                        }


                        ).ToList();

            }
        }
        public List<UEncapInventario> ConsultarInventarioPrecioCombinado(string valores, int marca, int categor)
        {
            //Metodo de Slip
            string[] split = valores.Split(',');
            int can1 = Convert.ToInt32(split[0]);
            int can2 = Convert.ToInt32(split[1]);


            using (var db = new Mapeo())
            {
                return (from uu in db.inventario.Where(x => x.Precio >= can1 && x.Precio <= can2 &&
                        x.Id_marca == marca && x.Id_categoria == categor)
                        join marca_carro in db.marca_carro on uu.Id_marca equals marca_carro.Id
                        join categoria in db.categoria on uu.Id_categoria equals categoria.Id
                        join estadoitem in db.estado_item on uu.Id_estado equals estadoitem.Id

                        let _cantCarrito = (from ss in db.carrito where ss.Producto_id == uu.Id select ss.Cantidad).Sum()

                        select new
                        {
                            uu,
                            marca_carro,
                            categoria,
                            estadoitem,

                            _cantCarrito

                        }).ToList().Select(m => new UEncapInventario

                        {
                            Id = m.uu.Id,
                            Imagen = m.uu.Imagen,
                            Titulo = m.uu.Titulo,
                            Precio = m.uu.Precio,
                            Referencia = m.uu.Referencia,
                            Ca_actual = m.uu.Ca_actual - (m._cantCarrito.HasValue ? m._cantCarrito.Value : 0),
                            Ca_minima = m.uu.Ca_minima,
                            Id_marca = m.uu.Id_marca,
                            Id_categoria = m.uu.Id_categoria,

                            Id_estado = m.uu.Id_estado,

                            Nombre_categoria = m.categoria.Categoria,
                            Nombre_marca = m.marca_carro.Marca,

                            Estado = m.estadoitem.Estado_item





                        }


                        ).ToList();

            }
        }
        public List<UEncapCategoria> ColsultarCategoria2()
        {
            using (var db = new Mapeo())
            {
                return db.categoria.OrderBy(x => x.Id >= 1).ToList();
            }
        }
        public List<UEncapMarca> ColsultarMarca()
        {
            using (var db = new Mapeo())
            {
                return db.marca_carro.OrderBy(x => x.Id).ToList();
            }
        }
        public UEncapUsuario verificarIdentificacion(UEncapUsuario verificar)
        {
            using (var db = new Mapeo())
            {
                return db.usuario.Where(x => x.Correo.Equals(verificar.Correo) || x.Identificacion.Equals(verificar.Identificacion)).FirstOrDefault();
            }
        }
        public void InsertarCliente(UEncapUsuario cliente)
        {
            using (var db = new Mapeo())
            {
                db.usuario.Add(cliente);
                db.SaveChanges();
            }
        }
        public void InsertarEmpleado(UEncapUsuario empleado)
        {
            using (var db = new Mapeo())
            {
                db.usuario.Add(empleado);
                db.SaveChanges();
            }
        }

    }
}
