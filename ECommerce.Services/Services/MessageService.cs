﻿namespace ECommerce.Services.Services;

public class MessageService(IHttpService http, ICookieService cookieService) : EntityService<Message>(http),
    IMessageService
{
    private const string Url = "api/Messages";
    private List<Message> _messages;

    public async Task<ServiceResult<List<Message>>> Load()
    {
        var result = await ReadList(Url);
        return Return(result);
    }

    public async Task<ServiceResult<List<Message>>> Filtering(string filter)
    {
        if (_messages == null)
        {
            var messages = await Load();
            if (messages.Code > 0) return messages;
            _messages = messages.ReturnData;
        }

        var result = _messages.Where(x => x.Name.Contains(filter)).ToList();
        if (result.Count == 0)
            return new ServiceResult<List<Message>> { Code = ServiceCode.Info, Message = "پیغامی یافت نشد" };
        return new ServiceResult<List<Message>>
        {
            Code = ServiceCode.Success,
            ReturnData = result
        };
    }

    public async Task<ServiceResult> Add(Message message)
    {
        var currentUser = cookieService.GetCurrentUser();
        if (currentUser.Id != 0) message.UserId = currentUser.Id;
        var result = await Create(Url, message);
        _messages = null;
        return Return(result);
    }

    public async Task<ServiceResult> Edit(Message message)
    {
        var result = await Update(Url, message);
        _messages = null;
        return Return(result);
    }

    public async Task<ServiceResult> Delete(int id)
    {
        var result = await Delete(Url, id);
        _messages = null;
        return Return(result);
    }
}