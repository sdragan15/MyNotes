using MyNotes.Application.Model;
using MyNotes.Domain.Entities;
using MyNotes.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Application.Services
{
    public class ItemService
    {
        public IItemRepository _itemRepository { get; }

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task AddItemAsync(ItemDto item)
        {
            try
            {
                var itemEntity = new Item
                {
                    Text = item.Text,
                    IsChecked = item.IsChecked,
                    DateCreated = DateTime.UtcNow
                };
                await _itemRepository.AddAsync(itemEntity);
            }
            catch(Exception ex)
            {
                var msg = ex.InnerException?.Message;
                throw new ApplicationException("An error occurred while adding the item.", ex);
            }

        }

        public async Task UpdateItemAsync(ItemDto item)
        {
            try
            {
                var existingItem = await _itemRepository.GetByIdAsync(item.Id);
                if (existingItem != null)
                {
                    existingItem.Text = item.Text;
                    existingItem.IsChecked = item.IsChecked;
                    await _itemRepository.UpdateAsync(existingItem);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the item.", ex);
            }

        }

        public async Task DeleteItemAsync(Guid itemId)
        {
            try
            {
                var existingItem = await _itemRepository.GetByIdAsync(itemId);
                if (existingItem != null)
                {
                    await _itemRepository.DeleteAsync(existingItem);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deleting the item.", ex);
            }
            
        }

        public async Task<List<ItemDto>> GetAllItemsAsync()
        {
            try
            {
                var items = await _itemRepository.GetAllAsync();
                var itemsDto = items.Select(item => new ItemDto
                {
                    Id = item.Id,
                    Text = item.Text,
                    IsChecked = item.IsChecked,
                    DateCreated = item.DateCreated
                }).ToList();

                return itemsDto;
            }
            catch(Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving items.", ex);
            }
           
        }
    }
}
