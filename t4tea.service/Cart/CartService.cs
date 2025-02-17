﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using t4tea.data.Entities;
using t4tea.repository.Interfaces;
using t4tea.service.Cart.Dtos;

namespace t4tea.service.Cart
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AddCart> AddCart(AddCart cartDto)
        {
            if (cartDto != null)
            {
                // تحويل الـ DTO إلى الكائن الذي سيتم حفظه في قاعدة البيانات
                var cartEntity = _mapper.Map<CartItems>(cartDto);


                // إضافة الكائن إلى Repository
                await _unitOfWork.Repository<CartItems>().AddAsync(cartEntity);

                // حفظ التغييرات في قاعدة البيانات
                var status = await _unitOfWork.CompleteAsync();

                if (status == 0)
                {
                    return null;
                }

                // يمكن العودة بالـ DTO أو الكائن الذي تم إضافته بعد تحويله مرة أخرى إذا لزم الأمر
                return cartDto; // أو يمكن إنشاء AddNewTeaDto جديد من الكائن الذي تم حفظه
            }

            return null; // في حالة لم يتم تمرير تفاصيل الشاي
        }

        public async Task<int> DeleteCart(int id)
        {
            var cart = await _unitOfWork.Repository<CartItems>().GetByIdAsync(id);
            if (cart != null)
            {
                _unitOfWork.Repository<CartItems>().Delete(cart);
                var status = await _unitOfWork.CompleteAsync(); //

                if (status == 0)
                {
                    return 0;
                }
                return status;
            }
            return 0;
        }

        public async Task<IReadOnlyList<GetCart>> GetAllCarts()
        {
            var cart = await _unitOfWork.Repository<CartItems>().GetAllCartsAsync();
            var mappedcart = _mapper.Map<IReadOnlyList<GetCart>>(cart);
            return mappedcart;
        }

        public async Task<GetCart> GetCartById(int id)
        {
            var cart = await _unitOfWork.Repository<CartItems>().GetCartByIdAsync(id);
            var mappedcart = _mapper.Map<GetCart>(cart);
            return mappedcart;
        }



        public async Task<GetCart> GetCartByIdWithoutInclude(int id)
        {
            var cart = await _unitOfWork.Repository<CartItems>().GetByIdAsync(id);
            var mappedcart = _mapper.Map<GetCart>(cart);
            return mappedcart;
        }

        public async Task<IReadOnlyList<GetCart>> GetCartByUserId(string id)
        {
            var cart = await _unitOfWork.Repository<CartItems>().GetByUserIdAsync(id);
            var mappedcart = _mapper.Map<IReadOnlyList<GetCart>>(cart);
            return mappedcart;
        }




        // بتمسح بدل ما تعدل
        public async Task<GetCart> UpdateCart(int id, AddCart cartDto)
        {
            var cart = await _unitOfWork.Repository<CartItems>().GetByIdAsync(id);

            if (cart != null)
            {
                // استخدام AutoMapper لتحويل الـ DTO إلى الكائن الفعلي (ElsaeedTeaProduct)
                var mappedCart = _mapper.Map<CartItems>(cartDto);

                // تحديث الكائن بالبيانات الجديدة
                cart.ProductId = mappedCart.ProductId;
                cart.Quantity = mappedCart.Quantity;
                cart.UserId = mappedCart.UserId;
                //cart.Product = mappedCart.Product;
                //cart.User = mappedCart.User;

                // تحديث الكائن في الـ Repository
                _unitOfWork.Repository<CartItems>().Update(cart);

                // حفظ التغييرات في قاعدة البيانات
                var status = await _unitOfWork.CompleteAsync();

                if (status == 0)
                {
                    return null;
                }

                var mapToDto = new GetCart()
                {
                    Id = id,
                    ProductId = mappedCart.ProductId,
                    Quantity = mappedCart.Quantity,
                    UserId = mappedCart.UserId,
                    User = mappedCart.User,
                    Product = mappedCart.Product
                };

                return mapToDto;
            }

            return null;
        }


    }
}
